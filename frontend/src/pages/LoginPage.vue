<template>
  <div class="login-container">
    <div class="login-card">
      <div class="login-header">
        <i class="pi pi-users" style="font-size: 3rem"></i>
        <h1>Contact Agenda</h1>
        <p>Sign in to manage your contacts</p>
      </div>

      <form @submit.prevent="handleSubmit" class="login-form">
        <div class="field">
          <label for="username">Username</label>
          <InputText
            id="username"
            v-model="formData.username"
            placeholder="Enter your username"
            :class="{ 'p-invalid': errors.username }"
            @blur="validateField('username')"
          />
          <small v-if="errors.username" class="p-error">{{ errors.username }}</small>
        </div>

        <div class="field">
          <label for="password">Password</label>
          <Password
            id="password"
            v-model="formData.password"
            placeholder="Enter your password"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': errors.password }"
            @blur="validateField('password')"
          />
          <small v-if="errors.password" class="p-error">{{ errors.password }}</small>
        </div>

        <Button
          type="submit"
          label="Sign In"
          icon="pi pi-sign-in"
          :loading="loading"
          class="w-full login-button"
        />

        <div class="register-link">
          Don't have an account?
          <router-link to="/register">Sign up here</router-link>
        </div>
      </form>
    </div>
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
      summary: 'Welcome back!',
      detail: `Logged in as ${authStore.user.username}`,
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
        summary: 'Login Failed',
        detail: err.response?.data?.message || 'Invalid username or password',
        life: 5000
      })
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 2rem;
}

.login-card {
  background: white;
  border-radius: 1rem;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  padding: 3rem;
  width: 100%;
  max-width: 450px;
  animation: fadeInUp 0.5s ease-out;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.login-header {
  text-align: center;
  margin-bottom: 2rem;
}

.login-header i {
  color: #667eea;
  margin-bottom: 1rem;
}

.login-header h1 {
  font-size: 2rem;
  margin: 0.5rem 0;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.login-header p {
  color: #64748b;
  margin: 0;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.field label {
  font-weight: 600;
  color: #334155;
}

.login-button {
  margin-top: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  padding: 0.75rem;
  font-size: 1rem;
  font-weight: 600;
}

.login-button:hover {
  background: linear-gradient(135deg, #5568d3 0%, #6a3f8f 100%);
}

.register-link {
  text-align: center;
  margin-top: 1rem;
  color: #64748b;
}

.register-link a {
  color: #667eea;
  font-weight: 600;
  text-decoration: none;
}

.register-link a:hover {
  text-decoration: underline;
}

.p-error {
  color: #ef4444;
  font-size: 0.875rem;
}

:deep(.p-inputtext),
:deep(.p-password-input) {
  width: 100%;
}
</style>
