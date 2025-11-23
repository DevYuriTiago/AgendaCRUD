import * as yup from 'yup'

export const loginSchema = yup.object({
  username: yup
    .string()
    .required('Nome de usuário é obrigatório')
    .min(3, 'Nome de usuário deve ter pelo menos 3 caracteres'),
  password: yup
    .string()
    .required('Senha é obrigatória')
    .min(6, 'Senha deve ter pelo menos 6 caracteres')
})

export const registerSchema = yup.object({
  username: yup
    .string()
    .required('Nome de usuário é obrigatório')
    .min(3, 'Nome de usuário deve ter pelo menos 3 caracteres')
    .max(50, 'Nome de usuário não pode exceder 50 caracteres')
    .matches(/^[a-zA-Z0-9_.-]+$/, 'Nome de usuário pode conter apenas letras, números, pontos, hífens e sublinhados'),
  email: yup
    .string()
    .required('E-mail é obrigatório')
    .email('Deve ser um e-mail válido')
    .max(254, 'E-mail não pode exceder 254 caracteres'),
  password: yup
    .string()
    .required('Senha é obrigatória')
    .min(6, 'Senha deve ter pelo menos 6 caracteres')
    .max(100, 'Senha não pode exceder 100 caracteres'),
  confirmPassword: yup
    .string()
    .required('Por favor confirme sua senha')
    .oneOf([yup.ref('password')], 'As senhas devem coincidir'),
  fullName: yup
    .string()
    .required('Nome completo é obrigatório')
    .min(3, 'Nome completo deve ter pelo menos 3 caracteres')
    .max(100, 'Nome completo não pode exceder 100 caracteres')
})
