<template>
  <div class="relative flex min-h-screen w-full items-center justify-center overflow-hidden bg-background">
    <!-- Background Pattern -->
    <div class="absolute inset-0 z-0">
      <div class="absolute inset-0 bg-[linear-gradient(to_right,#80808012_1px,transparent_1px),linear-gradient(to_bottom,#80808012_1px,transparent_1px)] bg-[size:24px_24px]"></div>
      <div class="absolute left-0 right-0 top-0 -z-10 m-auto h-[310px] w-[310px] rounded-full bg-primary/20 opacity-20 blur-[100px]"></div>
    </div>

    <MagicCard class="relative z-10 w-full max-w-md border-border/50 bg-card/50 backdrop-blur-xl">
      <div class="flex flex-col items-center space-y-2 text-center mb-8">
        <div class="rounded-full bg-primary/10 p-4 mb-2">
          <i class="pi pi-users text-3xl text-primary"></i>
        </div>
        <h1 class="text-2xl font-bold tracking-tight">Agenda de Contatos</h1>
        <p class="text-sm text-muted-foreground">Entre para gerenciar seus contatos</p>
      </div>

      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div class="space-y-2">
          <label for="username" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">Nome de Usuário</label>
          <InputText
            id="username"
            v-model="formData.username"
            placeholder="Digite seu nome de usuário"
            :class="{ 'p-invalid': errors.username }"
            class="w-full"
            @blur="validateField('username')"
          />
          <small v-if="errors.username" class="text-destructive text-xs">{{ errors.username }}</small>
        </div>

        <div class="space-y-2">
          <label for="password" class="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">Senha</label>
          <Password
            id="password"
            v-model="formData.password"
            placeholder="Digite sua senha"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': errors.password }"
            inputClass="w-full"
            class="w-full"
            @blur="validateField('password')"
          />
          <small v-if="errors.password" class="text-destructive text-xs">{{ errors.password }}</small>
        </div>

        <Button
          type="submit"
          label="Entrar"
          icon="pi pi-sign-in"
          :loading="loading"
          class="w-full mt-6"
        />

        <div class="text-center text-sm text-muted-foreground mt-4">
          Não tem uma conta?
          <router-link to="/register" class="font-medium text-primary hover:underline">Cadastre-se aqui</router-link>
        </div>
      </form>
    </MagicCard>

    <footer class="absolute bottom-4 text-center text-xs text-muted-foreground">
      <p>Agenda de Contatos. Feito com .NET + VUE</p>
    </footer>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'
import { useAuthStore } from '../store/authStore'
import { loginSchema } from '../utils/authValidation'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import MagicCard from '@/components/ui/MagicCard.vue'

const router = useRouter()
const toast = useToast()
const authStore = useAuthStore()

const formData = reactive({
  username: '',
  password: ''
})

const errors = reactive({})
const loading = ref(false)

const validateField = async (field) => {
  try {
    await loginSchema.validateAt(field, formData)
    errors[field] = ''
  } catch (err) {
    errors[field] = err.message
  }
}

const handleSubmit = async () => {
  try {
    await loginSchema.validate(formData, { abortEarly: false })
    Object.keys(errors).forEach(key => errors[key] = '')

    loading.value = true
    await authStore.login(formData.username, formData.password)

    toast.add({
      severity: 'success',
      summary: 'Bem-vindo de volta!',
      detail: `Conectado como ${authStore.user.username}`,
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
        summary: 'Erro no login',
        detail: err.message || 'Verifique suas credenciais',
        life: 3000
      })
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
/* Removed custom styles in favor of Tailwind */
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
