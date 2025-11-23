import api from './api'

const authService = {
  async register(username, email, password, fullName) {
    const response = await api.post('/auth/register', {
      username,
      email,
      password,
      fullName
    })
    return response.data
  },

  async login(username, password) {
    const response = await api.post('/auth/login', {
      username,
      password
    })
    return response.data
  },

  async refreshToken(refreshToken) {
    const response = await api.post('/auth/refresh', {
      refreshToken
    })
    return response.data
  },

  logout() {
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
    localStorage.removeItem('user')
  },

  saveTokens(accessToken, refreshToken) {
    localStorage.setItem('accessToken', accessToken)
    localStorage.setItem('refreshToken', refreshToken)
  },

  getAccessToken() {
    return localStorage.getItem('accessToken')
  },

  getRefreshToken() {
    return localStorage.getItem('refreshToken')
  },

  saveUser(user) {
    localStorage.setItem('user', JSON.stringify(user))
  },

  getUser() {
    const user = localStorage.getItem('user')
    return user ? JSON.parse(user) : null
  },

  isAuthenticated() {
    return !!this.getAccessToken()
  }
}

export default authService
