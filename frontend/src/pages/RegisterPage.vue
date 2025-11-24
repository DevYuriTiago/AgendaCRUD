<template>
  <div class="register-container">
    <div class="register-card">
      <div class="register-header">
        <i class="pi pi-user-plus" style="font-size: 3rem"></i>
        <h1>Criar Conta</h1>
        <p>Junte-se à Agenda de Contatos hoje</p>
      </div>

      <form @submit.prevent="handleSubmit" class="register-form">
        <div class="field">
          <label for="fullName">Nome Completo</label>
          <InputText
            id="fullName"
            v-model="formData.fullName"
            placeholder="Digite seu nome completo"
            :class="{ 'p-invalid': errors.fullName }"
            @blur="validateField('fullName')"
          />
          <small v-if="errors.fullName" class="p-error">{{ errors.fullName }}</small>
        </div>

        <div class="field">
          <label for="username">Nome de Usuário</label>
          <InputText
            id="username"
            v-model="formData.username"
            placeholder="Escolha um nome de usuário"
            :class="{ 'p-invalid': errors.username }"
            @blur="validateField('username')"
          />
          <small v-if="errors.username" class="p-error">{{ errors.username }}</small>
        </div>

        <div class="field">
          <label for="email">E-mail</label>
          <InputText
            id="email"
            v-model="formData.email"
            type="email"
            placeholder="Digite seu e-mail"
            :class="{ 'p-invalid': errors.email }"
            @blur="validateField('email')"
          />
          <small v-if="errors.email" class="p-error">{{ errors.email }}</small>
        </div>

        <div class="field">
          <label for="password">Senha</label>
          <PasswordStrengthIndicator
            id="password"
            v-model="formData.password"
            placeholder="Crie uma senha"
            :error-message="errors.password"
            @validate="handlePasswordValidation"
          />
        </div>

        <div class="field">
          <label for="confirmPassword">Confirmar Senha</label>
          <Password
            id="confirmPassword"
            v-model="formData.confirmPassword"
            placeholder="Confirme sua senha"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': errors.confirmPassword }"
            @blur="validateField('confirmPassword')"
          />
          <small v-if="errors.confirmPassword" class="p-error">{{ errors.confirmPassword }}</small>
        </div>

        <Button
          type="submit"
          label="Criar Conta"
          icon="pi pi-check"
          :loading="loading"
          :disabled="!isFormValid"
          class="w-full register-button"
        />

        <div class="login-link">
          Já tem uma conta?
          <router-link to="/login">Entre aqui</router-link>
        </div>
      </form>
    </div>

    <footer class="page-footer">
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
.register-container {
  @apply min-h-screen w-full flex items-center justify-center bg-slate-950 relative overflow-hidden p-8;
}

.register-container::before {
  content: '';
  @apply absolute w-[350px] h-[350px] rounded-full top-[-120px] right-[-120px] animate-[float_7s_ease-in-out_infinite];
  background: radial-gradient(circle, rgba(255,255,255,0.05) 0%, transparent 70%);
}

.register-container::after {
  content: '';
  @apply absolute w-[450px] h-[450px] rounded-full bottom-[-180px] left-[-180px] animate-[float_9s_ease-in-out_infinite_reverse];
  background: radial-gradient(circle, rgba(255,255,255,0.03) 0%, transparent 70%);
}

@keyframes float {
  0%, 100% { transform: translate(0, 0) rotate(0deg); }
  50% { transform: translate(25px, 25px) rotate(5deg); }
}

.register-card {
  @apply w-full max-w-[540px] p-12 relative z-10 rounded-3xl border border-white/10 shadow-2xl animate-[fadeInUp_0.6s_cubic-bezier(0.16,1,0.3,1)];
  background: rgba(15, 23, 42, 0.6);
  backdrop-filter: blur(20px);
  box-shadow: 0 25px 80px rgba(0, 0, 0, 0.5), 0 0 1px rgba(255, 255, 255, 0.1) inset;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(40px) scale(0.95);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

.register-header {
  @apply text-center mb-10;
}

.register-header i {
  @apply text-blue-500 mb-4 drop-shadow-[0_4px_12px_rgba(59,130,246,0.4)] animate-pulse;
}

.register-header h1 {
  @apply text-3xl my-2 font-bold bg-gradient-to-br from-blue-400 to-indigo-400 bg-clip-text text-transparent;
}

.register-header p {
  @apply text-slate-400 font-medium m-0;
}

.register-form {
  @apply flex flex-col gap-5;
}

.field {
  @apply flex flex-col gap-2;
}

.field label {
  @apply font-semibold text-slate-200 text-[0.95rem] tracking-wide;
}

:deep(.p-inputtext),
:deep(.p-password-input) {
  @apply w-full p-3.5 border-2 border-slate-700 rounded-xl text-base transition-all duration-300 bg-slate-800/50 text-white placeholder:text-slate-500;
}

:deep(.p-inputtext:hover),
:deep(.p-password-input:hover) {
  @apply border-slate-600 bg-slate-800;
}

:deep(.p-inputtext:focus),
:deep(.p-password-input:focus) {
  @apply border-blue-500 ring-4 ring-blue-500/20 bg-slate-800 -translate-y-[1px];
}

.register-button {
  @apply mt-4 bg-gradient-to-br from-blue-600 to-indigo-600 border-0 p-4 text-lg font-semibold rounded-xl transition-all duration-300 relative overflow-hidden shadow-lg shadow-blue-500/30 text-white;
}

.register-button::before {
  content: '';
  @apply absolute top-0 -left-full w-full h-full bg-gradient-to-r from-transparent via-white/30 to-transparent transition-[left] duration-500;
}

.register-button:hover::before {
  @apply left-full;
}

.register-button:hover {
  @apply -translate-y-0.5 shadow-xl shadow-blue-500/50;
}

.register-button:active {
  @apply translate-y-0 shadow-lg shadow-blue-500/30;
}

.register-button:disabled {
  @apply opacity-60 cursor-not-allowed transform-none shadow-none;
}

.login-link {
  @apply text-center mt-6 pt-6 border-t border-slate-800 text-slate-400 text-[0.95rem];
}

.login-link a {
  @apply text-blue-400 font-semibold no-underline transition-all duration-200 relative pb-[2px];
}

.login-link a::after {
  content: '';
  @apply absolute bottom-0 left-0 w-0 h-[2px] bg-gradient-to-r from-blue-400 to-indigo-400 transition-[width] duration-300;
}

.login-link a:hover::after {
  @apply w-full;
}

.login-link a:hover {
  @apply text-indigo-400;
}

.p-error {
  @apply text-red-400 text-sm;
}

:deep(.p-inputtext),
:deep(.p-password-input) {
  @apply w-full;
}

/* Responsividade completa */
@media (max-width: 768px) {
  .register-container {
    @apply p-8;
  }

  .register-card {
    @apply p-10 max-w-full;
  }

  .register-header h1 {
    @apply text-3xl;
  }

  .register-form {
    @apply gap-5;
  }

  .register-button {
    @apply p-3.5 text-base;
  }
}

@media (max-width: 480px) {
  .register-container {
    @apply p-6;
  }

  .register-card {
    @apply p-8 rounded-[20px];
  }

  .register-header {
    @apply mb-8;
  }

  .register-header h1 {
    @apply text-2xl;
  }

  .register-header p {
    @apply text-sm;
  }

  .register-form {
    @apply gap-4;
  }

  .field label {
    @apply text-sm;
  }

  :deep(.p-inputtext),
  :deep(.p-password-input) {
    @apply p-3 text-[0.95rem];
  }

  .register-button {
    @apply p-3 text-[0.95rem] mt-3;
  }

  .login-link {
    @apply text-sm mt-5 pt-5;
  }
}

@media (max-width: 360px) {
  .register-card {
    @apply p-6;
  }

  .register-header h1 {
    @apply text-[1.35rem];
  }

  .register-form {
    @apply gap-3.5;
  }
}

.page-footer {
  @apply absolute bottom-0 left-0 right-0 p-6 text-center text-white/90 font-medium text-[0.95rem] z-10 drop-shadow-md;
}

.page-footer p {
  @apply m-0;
}

@media (max-width: 768px) {
  .page-footer {
    @apply relative p-5 text-sm;
  }
}

@media (max-width: 480px) {
  .page-footer {
    @apply p-4 text-xs;
  }
}
</style>
