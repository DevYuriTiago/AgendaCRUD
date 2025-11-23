import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useContactStore } from '@/store/contactStore'
import { contactApi } from '@/services/api'

vi.mock('@/services/api', () => ({
  contactApi: {
    getAll: vi.fn(),
    getById: vi.fn(),
    create: vi.fn(),
    update: vi.fn(),
    delete: vi.fn()
  }
}))

describe('Contact Store', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
  })

  it('should fetch contacts successfully', async () => {
    const mockContacts = [
      { id: '1', name: 'John Doe', email: 'john@example.com', phone: '11987654321' },
      { id: '2', name: 'Jane Smith', email: 'jane@example.com', phone: '11998765432' }
    ]

    contactApi.getAll.mockResolvedValue({ data: mockContacts })

    const store = useContactStore()
    await store.fetchContacts()

    expect(store.contacts).toEqual(mockContacts)
    expect(store.contactCount).toBe(2)
    expect(store.loading).toBe(false)
  })

  it('should create contact successfully', async () => {
    const newContact = { name: 'New User', email: 'new@example.com', phone: '11999999999' }
    const createdContact = { id: '3', ...newContact }

    contactApi.create.mockResolvedValue({ data: createdContact })

    const store = useContactStore()
    const result = await store.createContact(newContact)

    expect(result).toEqual(createdContact)
    expect(store.contacts).toContainEqual(createdContact)
  })

  it('should update contact successfully', async () => {
    const existingContact = { id: '1', name: 'John Doe', email: 'john@example.com', phone: '11987654321' }
    const updatedData = { name: 'John Updated', email: 'john.updated@example.com', phone: '11987654321' }
    const updatedContact = { ...existingContact, ...updatedData }

    contactApi.update.mockResolvedValue({ data: updatedContact })

    const store = useContactStore()
    store.contacts = [existingContact]

    await store.updateContact('1', updatedData)

    expect(store.contacts[0]).toEqual(updatedContact)
  })

  it('should delete contact successfully', async () => {
    const contacts = [
      { id: '1', name: 'John Doe', email: 'john@example.com', phone: '11987654321' },
      { id: '2', name: 'Jane Smith', email: 'jane@example.com', phone: '11998765432' }
    ]

    contactApi.delete.mockResolvedValue({})

    const store = useContactStore()
    store.contacts = [...contacts]

    await store.deleteContact('1')

    expect(store.contacts).toHaveLength(1)
    expect(store.contacts[0].id).toBe('2')
  })

  it('should handle errors correctly', async () => {
    const errorMessage = 'Network error'
    contactApi.getAll.mockRejectedValue({ message: errorMessage })

    const store = useContactStore()

    try {
      await store.fetchContacts()
    } catch (error) {
      expect(store.error).toBe(errorMessage)
      expect(store.loading).toBe(false)
    }
  })

  it('should sort contacts by name', () => {
    const store = useContactStore()
    store.contacts = [
      { id: '2', name: 'Zoe', email: 'zoe@example.com', phone: '11999999999' },
      { id: '1', name: 'Alice', email: 'alice@example.com', phone: '11988888888' },
      { id: '3', name: 'Bob', email: 'bob@example.com', phone: '11977777777' }
    ]

    const sorted = store.sortedContacts

    expect(sorted[0].name).toBe('Alice')
    expect(sorted[1].name).toBe('Bob')
    expect(sorted[2].name).toBe('Zoe')
  })
})
