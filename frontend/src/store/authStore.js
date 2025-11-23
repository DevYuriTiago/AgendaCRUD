import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import authService from '../services/authService'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(authService.getUser())
  const accessToken = ref(authService.getAccessToken())
  const refreshToken = ref(authService.getRefreshToken())
  const loading = ref(false)
  const error = ref(null)

  const isAuthenticated = computed(() => !!accessToken.value)
  const userRoles = computed(() => user.value?.roles || [])
  const isAdmin = computed(() => userRoles.value.includes('Admin'))

  async function register(username, email, password, fullName) {
    loading.value = true
    error.value = null
    try {
      const response = await authService.register(username, email, password, fullName)
      
      accessToken.value = response.accessToken
      refreshToken.value = response.refreshToken
      user.value = {
        userId: response.userId,
        username: response.username,
        email: response.email,
        fullName: response.fullName,
        roles: response.roles
      }

      authService.saveTokens(response.accessToken, response.refreshToken)
      authService.saveUser(user.value)

      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Registration failed'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function login(username, password) {
    loading.value = true
    error.value = null
    try {
      const response = await authService.login(username, password)
      
      accessToken.value = response.accessToken
      refreshToken.value = response.refreshToken
      user.value = {
        userId: response.userId,
        username: response.username,
        email: response.email,
        fullName: response.fullName,
        roles: response.roles
      }

      authService.saveTokens(response.accessToken, response.refreshToken)
      authService.saveUser(user.value)

      return response
    } catch (err) {
      error.value = err.response?.data?.message || 'Login failed'
      throw err
    } finally {
      loading.value = false
    }
  }

  async function refresh() {
    if (!refreshToken.value) {
      logout()
      return false
    }

    try {
      const response = await authService.refreshToken(refreshToken.value)
      
      accessToken.value = response.accessToken
      refreshToken.value = response.refreshToken
      user.value = {
        userId: response.userId,
        username: response.username,
        email: response.email,
        fullName: response.fullName,
        roles: response.roles
      }

      authService.saveTokens(response.accessToken, response.refreshToken)
      authService.saveUser(user.value)

      return true
    } catch (err) {
      logout()
      return false
    }
  }

  function logout() {
    user.value = null
    accessToken.value = null
    refreshToken.value = null
    error.value = null
    authService.logout()
  }

  return {
    user,
    accessToken,
    refreshToken,
    loading,
    error,
    isAuthenticated,
    userRoles,
    isAdmin,
    register,
    login,
    refresh,
    logout
  }
})
