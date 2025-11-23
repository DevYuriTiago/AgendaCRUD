<template>
  <div class="register-container">
    <div class="register-card">
      <div class="register-header">
        <i class="pi pi-user-plus" style="font-size: 3rem"></i>
        <h1>Create Account</h1>
        <p>Join Contact Agenda today</p>
      </div>

      <form @submit.prevent="handleSubmit" class="register-form">
        <div class="field">
          <label for="fullName">Full Name</label>
          <InputText
            id="fullName"
            v-model="formData.fullName"
            placeholder="Enter your full name"
            :class="{ 'p-invalid': errors.fullName }"
            @blur="validateField('fullName')"
          />
          <small v-if="errors.fullName" class="p-error">{{ errors.fullName }}</small>
        </div>

        <div class="field">
          <label for="username">Username</label>
          <InputText
            id="username"
            v-model="formData.username"
            placeholder="Choose a username"
            :class="{ 'p-invalid': errors.username }"
            @blur="validateField('username')"
          />
          <small v-if="errors.username" class="p-error">{{ errors.username }}</small>
        </div>

        <div class="field">
          <label for="email">Email</label>
          <InputText
            id="email"
            v-model="formData.email"
            type="email"
            placeholder="Enter your email"
            :class="{ 'p-invalid': errors.email }"
            @blur="validateField('email')"
          />
          <small v-if="errors.email" class="p-error">{{ errors.email }}</small>
        </div>

        <div class="field">
          <label for="password">Password</label>
          <Password
            id="password"
            v-model="formData.password"
            placeholder="Create a password"
            toggleMask
            :class="{ 'p-invalid': errors.password }"
            @blur="validateField('password')"
          />
          <small v-if="errors.password" class="p-error">{{ errors.password }}</small>
        </div>

        <div class="field">
          <label for="confirmPassword">Confirm Password</label>
          <Password
            id="confirmPassword"
            v-model="formData.confirmPassword"
            placeholder="Confirm your password"
            :feedback="false"
            toggleMask
            :class="{ 'p-invalid': errors.confirmPassword }"
            @blur="validateField('confirmPassword')"
          />
          <small v-if="errors.confirmPassword" class="p-error">{{ errors.confirmPassword }}</small>
        </div>

        <Button
          type="submit"
          label="Create Account"
          icon="pi pi-check"
          :loading="loading"
          class="w-full register-button"
        />

        <div class="login-link">
          Already have an account?
          <router-link to="/login">Sign in here</router-link>
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
import { registerSchema } from '../utils/authValidation'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'

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
      summary: 'Registration Successful!',
      detail: 'Welcome to Contact Agenda',
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
        summary: 'Registration Failed',
        detail: err.response?.data?.message || 'Unable to create account',
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
}

.register-card {
  background: white;
  border-radius: 1rem;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
  padding: 3rem;
  width: 100%;
  max-width: 500px;
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

.register-header {
  text-align: center;
  margin-bottom: 2rem;
}

.register-header i {
  color: #667eea;
  margin-bottom: 1rem;
}

.register-header h1 {
  font-size: 2rem;
  margin: 0.5rem 0;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
}

.register-header p {
  color: #64748b;
  margin: 0;
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
  color: #334155;
}

.register-button {
  margin-top: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  padding: 0.75rem;
  font-size: 1rem;
  font-weight: 600;
}

.register-button:hover {
  background: linear-gradient(135deg, #5568d3 0%, #6a3f8f 100%);
}

.login-link {
  text-align: center;
  margin-top: 1rem;
  color: #64748b;
}

.login-link a {
  color: #667eea;
  font-weight: 600;
  text-decoration: none;
}

.login-link a:hover {
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
