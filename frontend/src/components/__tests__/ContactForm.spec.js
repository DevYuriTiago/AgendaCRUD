import { describe, it, expect, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import ContactForm from '@/components/ContactForm.vue'
import PrimeVue from 'primevue/config'

describe('ContactForm', () => {
  let wrapper

  beforeEach(() => {
    wrapper = mount(ContactForm, {
      global: {
        plugins: [PrimeVue]
      }
    })
  })

  it('should render form fields', () => {
    expect(wrapper.find('#name').exists()).toBe(true)
    expect(wrapper.find('#email').exists()).toBe(true)
    expect(wrapper.find('#phone').exists()).toBe(true)
  })

  it('should show "Create Contact" button in create mode', () => {
    const button = wrapper.find('button[type="submit"]')
    expect(button.text()).toContain('Create Contact')
  })

  it('should show "Update Contact" button in edit mode', async () => {
    await wrapper.setProps({
      contact: {
        id: '1',
        name: 'John Doe',
        email: 'john@example.com',
        phone: '11987654321'
      }
    })

    const button = wrapper.find('button[type="submit"]')
    expect(button.text()).toContain('Update Contact')
  })

  it('should populate form fields in edit mode', async () => {
    const contact = {
      id: '1',
      name: 'John Doe',
      email: 'john@example.com',
      phone: '11987654321'
    }

    await wrapper.setProps({ contact })
    await wrapper.vm.$nextTick()

    expect(wrapper.vm.formData.name).toBe('John Doe')
    expect(wrapper.vm.formData.email).toBe('john@example.com')
    expect(wrapper.vm.formData.phone).toBe('11987654321')
  })

  it('should emit cancel event when cancel button is clicked', async () => {
    const cancelButton = wrapper.findAll('button').find(btn => 
      btn.text().includes('Cancel')
    )
    
    await cancelButton.trigger('click')
    
    expect(wrapper.emitted('cancel')).toBeTruthy()
  })

  it('should validate required fields', async () => {
    const form = wrapper.find('form')
    await form.trigger('submit.prevent')
    await wrapper.vm.$nextTick()

    expect(wrapper.vm.errors.name).toBeDefined()
    expect(wrapper.vm.errors.email).toBeDefined()
    expect(wrapper.vm.errors.phone).toBeDefined()
  })

  it('should clear field error on input', async () => {
    wrapper.vm.errors = { name: 'Name is required' }
    
    await wrapper.vm.clearFieldError('name')
    
    expect(wrapper.vm.errors.name).toBeUndefined()
  })
})
