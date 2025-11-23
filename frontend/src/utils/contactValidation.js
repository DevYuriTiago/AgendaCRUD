import * as yup from 'yup'

export const contactSchema = yup.object({
  name: yup
    .string()
    .required('Name is required')
    .min(3, 'Name must be at least 3 characters')
    .max(100, 'Name must not exceed 100 characters')
    .trim(),
  
  email: yup
    .string()
    .required('Email is required')
    .email('Invalid email format')
    .max(254, 'Email must not exceed 254 characters')
    .lowercase()
    .trim(),
  
  phone: yup
    .string()
    .required('Phone is required')
    .matches(/^\d+$/, 'Phone must contain only digits')
    .min(8, 'Phone must have at least 8 digits')
    .max(15, 'Phone must not exceed 15 digits')
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
