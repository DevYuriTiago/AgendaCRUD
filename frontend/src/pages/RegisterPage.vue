<template>
  <div class="relative flex min-h-screen w-full items-center justify-center overflow-hidden bg-background">
    <!-- Background Pattern -->
    <div class="absolute inset-0 z-0">
      <div class="absolute inset-0 bg-[linear-gradient(to_right,#80808012_1px,transparent_1px),linear-gradient(to_bottom,#80808012_1px,transparent_1px)] bg-[size:24px_24px]"></div>
      <div class="absolute left-0 right-0 top-0 -z-10 m-auto h-[310px] w-[310px] rounded-full bg-primary/20 opacity-20 blur-[100px]"></div>
    </div>

    <MagicCard class="relative z-10 w-full max-w-md border-border/50 bg-card/50 backdrop-blur-xl my-8">
      <div class="flex flex-col items-center space-y-2 text-center mb-8">
        <div class="rounded-full bg-primary/10 p-4 mb-2">
          <i class="pi pi-user-plus text-3xl text-primary"></i>
        </div>
        <h1 class="text-2xl font-bold tracking-tight">Criar Conta</h1>
        <p class="text-sm text-muted-foreground">Junte-se à Agenda de Contatos hoje</p>
      </div>

      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div class="space-y-2">
          <label for="fullName" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">Nome Completo</label>
          <InputText
            id="fullName"
            v-model="formData.fullName"
            placeholder="Digite seu nome completo"
            :class="{ 'p-invalid': errors.fullName }"
            class="w-full"
            @blur="validateField('fullName')"
          />
          <small v-if="errors.fullName" class="text-destructive text-xs">{{ errors.fullName }}</small>
        </div>

        <div class="space-y-2">
          <label for="username" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">Nome de Usuário</label>
          <InputText
            id="username"
            v-model="formData.username"
            placeholder="Escolha um nome de usuário"
            :class="{ 'p-invalid': errors.username }"
            class="w-full"
            @blur="validateField('username')"
          />
          <small v-if="errors.username" class="text-destructive text-xs">{{ errors.username }}</small>
        </div>

        <div class="space-y-2">
          <label for="email" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">E-mail</label>
          <InputText
            id="email"
            v-model="formData.email"
            type="email"
            placeholder="Digite seu e-mail"
            :class="{ 'p-invalid': errors.email }"
            class="w-full"
            @blur="validateField('email')"
          />
          <small v-if="errors.email" class="text-destructive text-xs">{{ errors.email }}</small>
        </div>

        <div class="space-y-2">
          <label for="password" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">Senha</label>
          <PasswordStrengthIndicator
            id="password"
            v-model="formData.password"
            placeholder="Crie uma senha"
            :error-message="errors.password"
            @validate="handlePasswordValidation"
          />
        </div>

        <div class="space-y-2">
          <label for="confirmPassword" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">Confirmar Senha</label>
          <Password
            id="confirmPassword"
            v-model="formData.confirmPassword"
            placeholder="Confirme sua senha"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': errors.confirmPassword }"
            inputClass="w-full"
            class="w-full"
            @blur="validateField('confirmPassword')"
          />
          <small v-if="errors.confirmPassword" class="text-destructive text-xs">{{ errors.confirmPassword }}</small>
        </div>

        <Button
          type="submit"
          label="Criar Conta"
          icon="pi pi-check"
          :loading="loading"
          :disabled="!isFormValid"
          class="w-full mt-6"
        />

        <div class="text-center text-sm text-muted-foreground mt-4">
          Já tem uma conta?
          <router-link to="/login" class="font-medium text-primary hover:underline">Entre aqui</router-link>
        </div>
      </form>
    </MagicCard>

    <footer class="absolute bottom-4 text-center text-xs text-muted-foreground">
      <p>Agenda de Contatos. Feito com .NET + VUE</p>
    </footer>
  </div>
</template>

<script setup>
import { reactive, ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { useAuthStore } from '../store/authStore'
import { registerSchema } from '../utils/authValidation'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import PasswordStrengthIndicator from '../components/PasswordStrengthIndicator.vue'
import MagicCard from '@/components/ui/MagicCard.vue'

const router = useRouter()
const toast = useToast()
const authStore = useAuthStore()


const formData = reactive({
  fullName: '',
  username: '',
  email: '',
  password: '',
  confirmPassword: ''
})

const errors = reactive({})
const loading = ref(false)
const isPasswordValid = ref(false)

const isFormValid = computed(() => {
  return (
    formData.fullName.trim() !== '' &&
    formData.username.trim() !== '' &&
    formData.email.trim() !== '' &&
    formData.password !== '' &&
    formData.confirmPassword !== '' &&
    formData.password === formData.confirmPassword &&
    isPasswordValid.value &&
    Object.keys(errors).every(key => !errors[key])
  )
})

const handlePasswordValidation = (isValid) => {
  isPasswordValid.value = isValid
  if (!isValid && formData.password.length > 0) {
    errors.password = 'A senha não atende aos requisitos'
  } else {
    errors.password = ''
  }
}

const validateField = async (field) => {
  try {
    await registerSchema.validateAt(field, formData)
    errors[field] = ''
  } catch (err) {
    errors[field] = err.message
  }
}

const handleSubmit = async () => {
  try {
    await registerSchema.validate(formData, { abortEarly: false })
    Object.keys(errors).forEach(key => errors[key] = '')

    loading.value = true
    await authStore.register(
      formData.username,
      formData.email,
      formData.password,
      formData.fullName
    )

    toast.add({
      severity: 'success',
      summary: 'Registro Bem-sucedido!',
      detail: 'Bem-vindo à Agenda de Contatos',
      life: 3000
    })

    router.push('/contacts')
  } catch (err) {
    if (err.inner) {
      err.inner.forEach(error => {
        errors[error.path] = error.message
      })
    } else {
      toast.add({
        severity: 'error',
        summary: 'Falha no Registro',
        detail: err.response?.data?.message || 'Não foi possível criar a conta',
        life: 5000
      })
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
:deep(.p-inputtext),
:deep(.p-password-input) {
  @apply bg-slate-800/50 border-slate-700 text-white placeholder:text-slate-500;
}

:deep(.p-inputtext:hover),
:deep(.p-password-input:hover) {
  @apply bg-slate-800 border-slate-600;
}

:deep(.p-inputtext:focus),
:deep(.p-password-input:focus) {
  @apply bg-slate-800 border-blue-500 ring-4 ring-blue-500/20;
}
</style>
