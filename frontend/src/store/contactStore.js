import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { contactApi } from '@/services/api'

export const useContactStore = defineStore('contact', () => {
  // State
  const contacts = ref([])
  const loading = ref(false)
  const error = ref(null)

  // Getters
  const contactCount = computed(() => contacts.value.length)
  
  const sortedContacts = computed(() => {
    return [...contacts.value].sort((a, b) => 
      a.name.localeCompare(b.name)
    )
  })

  // Actions
  async function fetchContacts() {
    loading.value = true
    error.value = null
    
    try {
      const response = await contactApi.getAll()
      contacts.value = response.data
      return response.data
    } catch (err) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function getContactById(id) {
    loading.value = true
    error.value = null
    
    try {
      const response = await contactApi.getById(id)
      return response.data
    } catch (err) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function createContact(contactData) {
    loading.value = true
    error.value = null
    
    try {
      const response = await contactApi.create(contactData)
      contacts.value.push(response.data)
      return response.data
    } catch (err) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function updateContact(id, contactData) {
    loading.value = true
    error.value = null
    
    try {
      const response = await contactApi.update(id, contactData)
      const index = contacts.value.findIndex(c => c.id === id)
      if (index !== -1) {
        contacts.value[index] = response.data
      }
      return response.data
    } catch (err) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  async function deleteContact(id) {
    loading.value = true
    error.value = null
    
    try {
      await contactApi.delete(id)
      contacts.value = contacts.value.filter(c => c.id !== id)
    } catch (err) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  function clearError() {
    error.value = null
  }

  return {
    // State
    contacts,
    loading,
    error,
    // Getters
    contactCount,
    sortedContacts,
    // Actions
    fetchContacts,
    getContactById,
    createContact,
    updateContact,
    deleteContact,
    clearError
  }
})
