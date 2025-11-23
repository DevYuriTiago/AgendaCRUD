<template>
  <div class="contact-form">
    <form @submit.prevent="handleSubmit" novalidate>
      <div class="form-grid">
        <div class="form-field">
          <label for="name" class="form-label">
            <i class="pi pi-user"></i>
            Name *
          </label>
          <InputText
            id="name"
            v-model="formData.name"
            :class="{ 'p-invalid': errors.name }"
            placeholder="Enter full name"
            @input="clearFieldError('name')"
          />
          <small v-if="errors.name" class="p-error">{{ errors.name }}</small>
        </div>

        <div class="form-field">
          <label for="email" class="form-label">
            <i class="pi pi-envelope"></i>
            Email *
          </label>
          <InputText
            id="email"
            v-model="formData.email"
            :class="{ 'p-invalid': errors.email }"
            placeholder="email@example.com"
            type="email"
            @input="clearFieldError('email')"
          />
          <small v-if="errors.email" class="p-error">{{ errors.email }}</small>
        </div>

        <div class="form-field">
          <label for="phone" class="form-label">
            <i class="pi pi-phone"></i>
            Phone *
          </label>
          <InputMask
            id="phone"
            v-model="formData.phone"
            :class="{ 'p-invalid': errors.phone }"
            mask="(99) 99999-9999"
            placeholder="(00) 00000-0000"
            @input="clearFieldError('phone')"
          />
          <small v-if="errors.phone" class="p-error">{{ errors.phone }}</small>
        </div>
      </div>

      <div class="form-actions">
        <Button
          label="Cancel"
          icon="pi pi-times"
          class="p-button-text p-button-secondary"
          type="button"
          @click="handleCancel"
        />
        <Button
          :label="isEditMode ? 'Update Contact' : 'Create Contact'"
          :icon="isEditMode ? 'pi pi-check' : 'pi pi-plus'"
          class="p-button-primary"
          type="submit"
          :loading="loading"
        />
      </div>
    </form>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import InputText from 'primevue/inputtext'
import InputMask from 'primevue/inputmask'
import Button from 'primevue/button'
import { validateContact } from '@/utils/contactValidation'

const props = defineProps({
  contact: {
    type: Object,
    default: null
  },
  loading: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['submit', 'cancel'])

const formData = ref({
  name: '',
  email: '',
  phone: ''
})

const errors = ref({})

const isEditMode = computed(() => !!props.contact)

// Watch for contact prop changes (edit mode)
watch(
  () => props.contact,
  (newContact) => {
    if (newContact) {
      formData.value = {
        name: newContact.name || '',
        email: newContact.email || '',
        phone: newContact.phone || ''
      }
    } else {
      resetForm()
    }
  },
  { immediate: true }
)

async function handleSubmit() {
  errors.value = {}

  // Validate form
  const validation = await validateContact(formData.value)
  
  if (!validation.valid) {
    errors.value = validation.errors
    return
  }

  // Normalize phone (remove formatting)
  const submitData = {
    ...formData.value,
    phone: formData.value.phone.replace(/\D/g, '')
  }

  emit('submit', submitData)
}

function handleCancel() {
  resetForm()
  emit('cancel')
}

function resetForm() {
  formData.value = {
    name: '',
    email: '',
    phone: ''
  }
  errors.value = {}
}

function clearFieldError(field) {
  if (errors.value[field]) {
    delete errors.value[field]
  }
}
</script>

<style scoped>
.contact-form {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.08);
}

.form-grid {
  display: grid;
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.form-field {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 600;
  color: #495057;
  font-size: 0.95rem;
}

.form-label i {
  color: #667eea;
  font-size: 0.875rem;
}

:deep(.p-inputtext),
:deep(.p-inputmask) {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 2px solid #e9ecef;
  border-radius: 8px;
  transition: all 0.3s ease;
  font-size: 1rem;
}

:deep(.p-inputtext:hover),
:deep(.p-inputmask:hover) {
  border-color: #667eea;
}

:deep(.p-inputtext:focus),
:deep(.p-inputmask:focus) {
  border-color: #667eea;
  box-shadow: 0 0 0 0.2rem rgba(102, 126, 234, 0.25);
}

:deep(.p-inputtext.p-invalid),
:deep(.p-inputmask.p-invalid) {
  border-color: #e24c4c;
}

.p-error {
  color: #e24c4c;
  font-size: 0.875rem;
  margin-top: 0.25rem;
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding-top: 1rem;
  border-top: 1px solid #e9ecef;
}

:deep(.p-button) {
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.3s ease;
}

:deep(.p-button-primary) {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
}

:deep(.p-button-primary:hover) {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

@media (max-width: 768px) {
  .form-actions {
    flex-direction: column-reverse;
  }

  :deep(.p-button) {
    width: 100%;
  }
}
</style>
