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
  @apply bg-transparent p-10;
}

.form-grid {
  @apply grid gap-7 mb-8;
}

.form-field {
  @apply flex flex-col gap-2.5 animate-[fadeInUp_0.4s_ease-out_backwards];
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
  @apply flex items-center gap-2.5 font-bold text-slate-200 text-[0.95rem] tracking-wide;
}

.form-label i {
  @apply text-blue-400 text-base;
}

:deep(.p-inputtext),
:deep(.p-inputmask) {
  @apply w-full p-3.5 border-2 border-slate-700 rounded-xl transition-all duration-300 text-base bg-slate-800/50 text-white placeholder:text-slate-500;
}

:deep(.p-inputtext:hover),
:deep(.p-inputmask:hover) {
  @apply border-slate-600 bg-slate-800;
}

:deep(.p-inputtext:focus),
:deep(.p-inputmask:focus) {
  @apply border-blue-500 ring-4 ring-blue-500/20 bg-slate-800 -translate-y-[1px];
}

:deep(.p-inputtext.p-invalid),
:deep(.p-inputmask.p-invalid) {
  @apply border-red-500 bg-red-900/10;
}

.p-error {
  @apply text-red-400 text-sm mt-1 flex items-center gap-1.5 font-medium;
}

.form-actions {
  @apply flex justify-end gap-4 pt-6 border-t-2 border-slate-800 animate-[fadeInUp_0.4s_ease-out_0.2s_backwards];
}

:deep(.p-button) {
  @apply py-3.5 px-7 rounded-xl font-semibold transition-all duration-300 text-base;
}

:deep(.p-button-text) {
  @apply text-slate-400 hover:bg-slate-800 hover:text-slate-200;
}

:deep(.p-button-primary) {
  @apply bg-gradient-to-br from-blue-600 to-indigo-600 border-0 shadow-lg shadow-blue-500/30 relative overflow-hidden;
}

:deep(.p-button-primary::before) {
  content: '';
  @apply absolute top-0 -left-full w-full h-full bg-gradient-to-r from-transparent via-white/30 to-transparent transition-[left] duration-500;
}

:deep(.p-button-primary:hover::before) {
  @apply left-full;
}

:deep(.p-button-primary:hover) {
  @apply -translate-y-0.5 shadow-xl shadow-blue-500/50;
}

:deep(.p-button-primary:active) {
  @apply translate-y-0;
}

@media (max-width: 768px) {
  .contact-form {
    @apply p-6;
  }

  .form-grid {
    @apply gap-6;
  }

  .form-actions {
    @apply flex-col-reverse gap-3.5;
  }

  :deep(.p-button) {
    @apply w-full justify-center;
  }
}

@media (max-width: 480px) {
  .contact-form {
    @apply p-5;
  }

  .form-grid {
    @apply gap-5 mb-6;
  }

  .form-label {
    @apply text-sm;
  }

  :deep(.p-inputtext),
  :deep(.p-inputmask) {
    @apply py-3 px-3.5 text-[0.95rem];
  }

  :deep(.p-button) {
    @apply py-3 px-6 text-[0.95rem];
  }

  .form-actions {
    @apply pt-5;
  }
}
</style>
