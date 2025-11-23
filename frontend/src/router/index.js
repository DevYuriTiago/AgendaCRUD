import { createRouter, createWebHistory } from 'vue-router'
import ContactsPage from '@/pages/ContactsPage.vue'
import LoginPage from '@/pages/LoginPage.vue'
import RegisterPage from '@/pages/RegisterPage.vue'
import { useAuthStore } from '@/store/authStore'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: LoginPage,
      meta: { title: 'Login', requiresGuest: true }
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterPage,
      meta: { title: 'Register', requiresGuest: true }
    },
    {
      path: '/',
      redirect: '/contacts'
    },
    {
      path: '/contacts',
      name: 'contacts',
      component: ContactsPage,
      meta: { title: 'Contacts', requiresAuth: true }
    }
  ]
})

router.beforeEach((to, from, next) => {
  document.title = to.meta.title ? `${to.meta.title} - Contact Agenda` : 'Contact Agenda'
  
  const authStore = useAuthStore()
  const isAuthenticated = authStore.isAuthenticated

  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/login')
  } else if (to.meta.requiresGuest && isAuthenticated) {
    next('/contacts')
  } else {
    next()
  }
})

export default router
