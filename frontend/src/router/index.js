import { createRouter, createWebHistory } from 'vue-router'
import ContactsPage from '@/pages/ContactsPage.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'contacts',
      component: ContactsPage,
      meta: { title: 'Contacts' }
    }
  ]
})

router.beforeEach((to, from, next) => {
  document.title = to.meta.title ? `${to.meta.title} - Contact Agenda` : 'Contact Agenda'
  next()
})

export default router
