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
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 2rem;
  position: relative;
  overflow: hidden;
}

.register-container::before {
  content: '';
  position: absolute;
  width: 350px;
  height: 350px;
  background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
  border-radius: 50%;
  top: -120px;
  right: -120px;
  animation: float 7s ease-in-out infinite;
}

.register-container::after {
  content: '';
  position: absolute;
  width: 450px;
  height: 450px;
  background: radial-gradient(circle, rgba(255,255,255,0.08) 0%, transparent 70%);
  border-radius: 50%;
  bottom: -180px;
  left: -180px;
  animation: float 9s ease-in-out infinite reverse;
}

@keyframes float {
  0%, 100% { transform: translate(0, 0) rotate(0deg); }
  50% { transform: translate(25px, 25px) rotate(5deg); }
}

.register-card {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border-radius: 24px;
  box-shadow: 0 25px 80px rgba(0, 0, 0, 0.25), 0 0 1px rgba(255, 255, 255, 0.5) inset;
  padding: 3rem;
  width: 100%;
  max-width: 540px;
  animation: fadeInUp 0.6s cubic-bezier(0.16, 1, 0.3, 1);
  position: relative;
  z-index: 1;
  border: 1px solid rgba(255, 255, 255, 0.3);
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
  text-align: center;
  margin-bottom: 2.5rem;
}

.register-header i {
  color: #667eea;
  margin-bottom: 1rem;
  filter: drop-shadow(0 4px 12px rgba(102, 126, 234, 0.4));
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { transform: scale(1); }
  50% { transform: scale(1.05); }
}

.register-header h1 {
  font-size: 2rem;
  margin: 0.5rem 0;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-weight: 700;
}

.register-header p {
  color: #718096;
  margin: 0;
  font-weight: 500;
}

.register-form {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.field label {
  font-weight: 600;
  color: #2d3748;
  font-size: 0.95rem;
  letter-spacing: 0.3px;
}

:deep(.p-inputtext),
:deep(.p-password-input) {
  width: 100%;
  padding: 0.875rem 1rem;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  background: #f8fafc;
}

:deep(.p-inputtext:hover),
:deep(.p-password-input:hover) {
  border-color: #cbd5e0;
  background: #fff;
}

:deep(.p-inputtext:focus),
:deep(.p-password-input:focus) {
  border-color: #667eea;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.12), 0 4px 12px rgba(102, 126, 234, 0.15);
  background: #fff;
  transform: translateY(-1px);
}

.register-button {
  margin-top: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  padding: 1rem;
  font-size: 1.1rem;
  font-weight: 600;
  border-radius: 12px;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  overflow: hidden;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
}

.register-button::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
  transition: left 0.5s;
}

.register-button:hover::before {
  left: 100%;
}

.register-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 35px rgba(102, 126, 234, 0.5);
}

.register-button:active {
  transform: translateY(0);
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
}

.register-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none !important;
  box-shadow: none !important;
}

.login-link {
  text-align: center;
  margin-top: 1.5rem;
  padding-top: 1.5rem;
  border-top: 1px solid #e2e8f0;
  color: #718096;
  font-size: 0.95rem;
}

.login-link a {
  color: #667eea;
  font-weight: 600;
  text-decoration: none;
  transition: all 0.2s ease;
  position: relative;
  padding-bottom: 2px;
}

.login-link a::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 0;
  height: 2px;
  background: linear-gradient(90deg, #667eea, #764ba2);
  transition: width 0.3s ease;
}

.login-link a:hover::after {
  width: 100%;
}

.login-link a:hover {
  color: #764ba2;
}

.p-error {
  color: #ef4444;
  font-size: 0.875rem;
}

:deep(.p-inputtext),
:deep(.p-password-input) {
  width: 100%;
}

/* Responsividade completa */
@media (max-width: 768px) {
  .register-container {
    padding: 2rem 1.5rem;
  }

  .register-card {
    padding: 2.5rem 2rem;
    max-width: 100%;
  }

  .register-header h1 {
    font-size: 1.75rem;
  }

  .register-header i {
    font-size: 2.5rem !important;
  }

  .register-form {
    gap: 1.15rem;
  }

  .register-button {
    padding: 0.875rem;
    font-size: 1rem;
  }
}

@media (max-width: 480px) {
  .register-container {
    padding: 1.5rem 1rem;
  }

  .register-card {
    padding: 2rem 1.5rem;
    border-radius: 20px;
  }

  .register-header {
    margin-bottom: 2rem;
  }

  .register-header h1 {
    font-size: 1.5rem;
  }

  .register-header i {
    font-size: 2.25rem !important;
  }

  .register-header p {
    font-size: 0.9rem;
  }

  .register-form {
    gap: 1rem;
  }

  .field label {
    font-size: 0.875rem;
  }

  :deep(.p-inputtext),
  :deep(.p-password-input) {
    padding: 0.75rem 0.875rem;
    font-size: 0.95rem;
  }

  .register-button {
    padding: 0.75rem;
    font-size: 0.95rem;
    margin-top: 0.75rem;
  }

  .login-link {
    font-size: 0.875rem;
    margin-top: 1.25rem;
    padding-top: 1.25rem;
  }
}

@media (max-width: 360px) {
  .register-card {
    padding: 1.5rem 1.25rem;
  }

  .register-header h1 {
    font-size: 1.35rem;
  }

  .register-form {
    gap: 0.875rem;
  }
}
</style>
