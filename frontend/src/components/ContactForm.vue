<template>
  <div class="contact-form">
    <form @submit.prevent="handleSubmit" novalidate>
      <div class="form-grid">
        <div class="form-field">
          <label for="name" class="form-label">
            <i class="pi pi-user"></i>
            Nome *
          </label>
          <InputText
            id="name"
            v-model="formData.name"
            :class="{ 'p-invalid': errors.name }"
            placeholder="Digite o nome completo"
            @input="clearFieldError('name')"
          />
          <small v-if="errors.name" class="p-error">{{ errors.name }}</small>
        </div>

        <div class="form-field">
          <label for="email" class="form-label">
            <i class="pi pi-envelope"></i>
            E-mail *
          </label>
          <InputText
            id="email"
            v-model="formData.email"
            :class="{ 'p-invalid': errors.email }"
            placeholder="email@exemplo.com"
            type="email"
            @input="clearFieldError('email')"
          />
          <small v-if="errors.email" class="p-error">{{ errors.email }}</small>
        </div>

        <div class="form-field">
          <label for="phone" class="form-label">
            <i class="pi pi-phone"></i>
            Telefone *
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
          label="Cancelar"
          icon="pi pi-times"
          class="p-button-text p-button-secondary"
          type="button"
          @click="handleCancel"
        />
        <Button
          :label="isEditMode ? 'Atualizar Contato' : 'Criar Contato'"
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
  border-radius: 0;
  padding: 2.5rem;
}

.form-grid {
  display: grid;
  gap: 1.75rem;
  margin-bottom: 2rem;
}

.form-field {
  display: flex;
  flex-direction: column;
  gap: 0.625rem;
  animation: fadeInUp 0.4s ease-out backwards;
}

.form-field:nth-child(1) { animation-delay: 0.05s; }
.form-field:nth-child(2) { animation-delay: 0.1s; }
.form-field:nth-child(3) { animation-delay: 0.15s; }

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(15px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.form-label {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  font-weight: 700;
  color: #334155;
  font-size: 0.95rem;
  letter-spacing: 0.2px;
}

.form-label i {
  color: #667eea;
  font-size: 1rem;
}

:deep(.p-inputtext),
:deep(.p-inputmask) {
  width: 100%;
  padding: 0.875rem 1rem;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  font-size: 1rem;
  background: #f8fafc;
}

:deep(.p-inputtext:hover),
:deep(.p-inputmask:hover) {
  border-color: #cbd5e0;
  background: white;
}

:deep(.p-inputtext:focus),
:deep(.p-inputmask:focus) {
  border-color: #667eea;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.12), 0 4px 12px rgba(102, 126, 234, 0.15);
  background: white;
  transform: translateY(-1px);
}

:deep(.p-inputtext.p-invalid),
:deep(.p-inputmask.p-invalid) {
  border-color: #ef4444;
  background: #fef2f2;
}

.p-error {
  color: #ef4444;
  font-size: 0.875rem;
  margin-top: 0.25rem;
  display: flex;
  align-items: center;
  gap: 0.375rem;
  font-weight: 500;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  padding-top: 1.5rem;
  border-top: 2px solid #f1f5f9;
  animation: fadeInUp 0.4s ease-out 0.2s backwards;
}

:deep(.p-button) {
  padding: 0.875rem 1.75rem;
  border-radius: 12px;
  font-weight: 600;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  font-size: 1rem;
}

:deep(.p-button-text) {
  color: #64748b;
}

:deep(.p-button-text:hover) {
  background: #f1f5f9;
  color: #334155;
}

:deep(.p-button-primary) {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.35);
  position: relative;
  overflow: hidden;
}

:deep(.p-button-primary::before) {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
  transition: left 0.5s;
}

:deep(.p-button-primary:hover::before) {
  left: 100%;
}

:deep(.p-button-primary:hover) {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.5);
}

:deep(.p-button-primary:active) {
  transform: translateY(0);
}

@media (max-width: 768px) {
  .contact-form {
    padding: 1.5rem;
  }

  .form-actions {
    flex-direction: column-reverse;
  }

  :deep(.p-button) {
    width: 100%;
  }
}
</style>
