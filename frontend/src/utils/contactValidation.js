import * as yup from 'yup'

export const contactSchema = yup.object({
  name: yup
    .string()
    .required('Nome é obrigatório')
    .min(3, 'Nome deve ter pelo menos 3 caracteres')
    .max(100, 'Nome não pode exceder 100 caracteres')
    .trim(),
  
  email: yup
    .string()
    .required('E-mail é obrigatório')
    .email('Formato de e-mail inválido')
    .max(254, 'E-mail não pode exceder 254 caracteres')
    .lowercase()
    .trim(),
  
  phone: yup
    .string()
    .required('Telefone é obrigatório')
    .matches(/^\d+$/, 'Telefone deve conter apenas dígitos')
    .min(8, 'Telefone deve ter pelo menos 8 dígitos')
    .max(15, 'Telefone não pode exceder 15 dígitos')
    .transform((value) => value.replace(/\D/g, ''))
})

export async function validateContact(data) {
  try {
    await contactSchema.validate(data, { abortEarly: false })
    return { valid: true, errors: {} }
  } catch (err) {
    const errors = {}
    err.inner.forEach((error) => {
      errors[error.path] = error.message
    })
    return { valid: false, errors }
  }
}
