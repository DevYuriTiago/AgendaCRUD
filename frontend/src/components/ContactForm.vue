<template>
  <form @submit.prevent="handleSubmit" novalidate class="space-y-4">
    <div class="space-y-2">
      <label for="name" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 flex items-center gap-2">
        <i class="pi pi-user text-primary"></i>
        Nome *
      </label>
      <InputText
        id="name"
        v-model="formData.name"
        :class="{ 'p-invalid': errors.name }"
        placeholder="Digite o nome completo"
        class="w-full"
        @input="clearFieldError('name')"
      />
      <small v-if="errors.name" class="text-destructive text-xs">{{ errors.name }}</small>
    </div>

    <div class="space-y-2">
      <label for="email" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 flex items-center gap-2">
        <i class="pi pi-envelope text-primary"></i>
        E-mail *
      </label>
      <InputText
        id="email"
        v-model="formData.email"
        :class="{ 'p-invalid': errors.email }"
        placeholder="email@exemplo.com"
        type="email"
        class="w-full"
        @input="clearFieldError('email')"
      />
      <small v-if="errors.email" class="text-destructive text-xs">{{ errors.email }}</small>
    </div>

    <div class="space-y-2">
      <label for="phone" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70 flex items-center gap-2">
        <i class="pi pi-phone text-primary"></i>
        Telefone *
      </label>
      <InputMask
        id="phone"
        v-model="formData.phone"
        :class="{ 'p-invalid': errors.phone }"
        mask="(99) 99999-9999"
        placeholder="(00) 00000-0000"
        class="w-full"
        @input="clearFieldError('phone')"
      />
      <small v-if="errors.phone" class="text-destructive text-xs">{{ errors.phone }}</small>
    </div>

    <div class="flex justify-end gap-3 pt-4">
      <Button
        label="Cancelar"
        icon="pi pi-times"
        severity="secondary"
        text
        type="button"
        @click="handleCancel"
      />
      <Button
        :label="isEditMode ? 'Atualizar Contato' : 'Criar Contato'"
        :icon="isEditMode ? 'pi pi-check' : 'pi pi-plus'"
        type="submit"
        :loading="loading"
      />
    </div>
  </form>
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
:deep(.p-inputtext),
:deep(.p-inputmask) {
  @apply bg-slate-800/50 border-slate-700 text-white placeholder:text-slate-500;
}

:deep(.p-inputtext:hover),
:deep(.p-inputmask:hover) {
  @apply bg-slate-800 border-slate-600;
}

:deep(.p-inputtext:focus),
:deep(.p-inputmask:focus) {
  @apply bg-slate-800 border-blue-500 ring-4 ring-blue-500/20;
}

:deep(.p-inputtext.p-invalid),
:deep(.p-inputmask.p-invalid) {
  @apply border-red-500 bg-red-900/10;
}
</style>
