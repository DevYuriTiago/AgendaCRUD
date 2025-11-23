<template>
  <div class="password-field-wrapper">
    <div class="password-input-container">
      <Password
        :id="id"
        v-model="localPassword"
        :placeholder="placeholder"
        toggleMask
        :feedback="false"
        :class="{ 'p-invalid': showError && !isValid }"
        @input="handleInput"
        @focus="showTooltip = true"
        @blur="handleBlur"
      />
      
      <!-- Tooltip com requisitos -->
      <div v-if="showTooltip" class="password-tooltip">
        <div class="tooltip-header">Requisitos da Senha:</div>
        <div class="requirement-list">
          <div class="requirement-item" :class="{ 'valid': requirements.minLength }">
            <i :class="requirements.minLength ? 'pi pi-check-circle' : 'pi pi-times-circle'"></i>
            <span>Mínimo de 8 caracteres</span>
          </div>
          <div class="requirement-item" :class="{ 'valid': requirements.hasUppercase }">
            <i :class="requirements.hasUppercase ? 'pi pi-check-circle' : 'pi pi-times-circle'"></i>
            <span>Uma letra maiúscula (A-Z)</span>
          </div>
          <div class="requirement-item" :class="{ 'valid': requirements.hasLowercase }">
            <i :class="requirements.hasLowercase ? 'pi pi-check-circle' : 'pi pi-times-circle'"></i>
            <span>Uma letra minúscula (a-z)</span>
          </div>
          <div class="requirement-item" :class="{ 'valid': requirements.hasSpecial }">
            <i :class="requirements.hasSpecial ? 'pi pi-check-circle' : 'pi pi-times-circle'"></i>
            <span>Um caractere especial (@$!%*?&#)</span>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Mensagem de erro -->
    <small v-if="showError && errorMessage" class="p-error">{{ errorMessage }}</small>
  </div>
</template>

<script setup>
import { ref, reactive, computed, watch } from 'vue'
import Password from 'primevue/password'

const props = defineProps({
  id: {
    type: String,
    required: true
  },
  modelValue: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: 'Digite a senha'
  },
  errorMessage: {
    type: String,
    default: ''
  }
})

const emit = defineEmits(['update:modelValue', 'validate'])

const localPassword = ref(props.modelValue)
const showTooltip = ref(false)
const showError = ref(false)

const requirements = reactive({
  minLength: false,
  hasUppercase: false,
  hasLowercase: false,
  hasSpecial: false
})

const isValid = computed(() => {
  return requirements.minLength &&
         requirements.hasUppercase &&
         requirements.hasLowercase &&
         requirements.hasSpecial
})

const validatePassword = (password) => {
  requirements.minLength = password.length >= 8
  requirements.hasUppercase = /[A-Z]/.test(password)
  requirements.hasLowercase = /[a-z]/.test(password)
  requirements.hasSpecial = /[@$!%*?&#]/.test(password)
  
  return isValid.value
}

const handleInput = () => {
  validatePassword(localPassword.value)
  emit('update:modelValue', localPassword.value)
  emit('validate', isValid.value)
}

const handleBlur = () => {
  showTooltip.value = false
  showError.value = localPassword.value.length > 0
}

// Watch para atualizar quando o valor externo mudar
watch(() => props.modelValue, (newVal) => {
  localPassword.value = newVal
  if (newVal) {
    validatePassword(newVal)
  }
})

// Validar valor inicial
if (props.modelValue) {
  validatePassword(props.modelValue)
}
</script>

<style scoped>
.password-field-wrapper {
  position: relative;
  width: 100%;
}

.password-input-container {
  position: relative;
  width: 100%;
}

:deep(.p-password) {
  width: 100%;
}

:deep(.p-password-input) {
  width: 100%;
}

.password-tooltip {
  position: absolute;
  top: calc(100% + 8px);
  left: 0;
  right: 0;
  background: white;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  animation: slideDown 0.2s ease-out;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.tooltip-header {
  font-weight: 600;
  color: #334155;
  margin-bottom: 8px;
  font-size: 0.875rem;
}

.requirement-list {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.requirement-item {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 0.875rem;
  color: #ef4444;
  transition: color 0.2s ease;
}

.requirement-item.valid {
  color: #22c55e;
}

.requirement-item i {
  font-size: 0.875rem;
  flex-shrink: 0;
}

.requirement-item.valid i {
  color: #22c55e;
}

.requirement-item:not(.valid) i {
  color: #ef4444;
}

.p-error {
  color: #ef4444;
  font-size: 0.875rem;
  display: block;
  margin-top: 0.25rem;
}

:deep(.p-invalid) {
  border-color: #ef4444 !important;
}

:deep(.p-invalid:focus) {
  box-shadow: 0 0 0 0.2rem rgba(239, 68, 68, 0.25) !important;
}
</style>
