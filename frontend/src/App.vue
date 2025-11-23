<template>
  <div id="app">
    <Toast />
    <ConfirmDialog />
    
    <div class="layout-wrapper">
      <header v-if="authStore.isAuthenticated" class="app-header">
        <div class="container">
          <div class="header-content">
            <div class="logo">
              <i class="pi pi-users"></i>
              <h1>Contact Agenda</h1>
            </div>
            <nav class="nav-menu">
              <router-link to="/contacts" class="nav-link">
                <i class="pi pi-th-large"></i>
                <span>Contacts</span>
              </router-link>
              <div class="user-menu">
                <Button
                  :label="authStore.user?.fullName"
                  icon="pi pi-user"
                  text
                  class="user-button"
                  @click="toggleMenu"
                  aria-haspopup="true"
                  aria-controls="overlay_menu"
                />
                <Menu ref="menu" id="overlay_menu" :model="menuItems" :popup="true" />
              </div>
            </nav>
          </div>
        </div>
      </header>

      <main class="app-main">
        <div :class="{ container: authStore.isAuthenticated }">
          <router-view />
        </div>
      </main>

      <footer v-if="authStore.isAuthenticated" class="app-footer">
        <div class="container">
          <p>&copy; 2025 Contact Agenda. Built with Vue 3 + PrimeVue</p>
        </div>
      </footer>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import { useAuthStore } from './store/authStore'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import Button from 'primevue/button'
import Menu from 'primevue/menu'

const router = useRouter()
const confirm = useConfirm()
const toast = useToast()
const authStore = useAuthStore()
const menu = ref()

const menuItems = ref([
  {
    label: 'Profile',
    icon: 'pi pi-user',
    command: () => {
      toast.add({ severity: 'info', summary: 'Profile', detail: 'Profile feature coming soon', life: 3000 })
    }
  },
  {
    separator: true
  },
  {
    label: 'Logout',
    icon: 'pi pi-sign-out',
    command: () => {
      confirm.require({
        message: 'Are you sure you want to logout?',
        header: 'Logout Confirmation',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          authStore.logout()
          toast.add({ severity: 'success', summary: 'Logged out', detail: 'See you soon!', life: 3000 })
          router.push('/login')
        }
      })
    }
  }
])

const toggleMenu = (event) => {
  menu.value.toggle(event)
}
</script>

<style scoped>
.layout-wrapper {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}
.app-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5rem 0;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.logo {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.logo i {
  font-size: 2rem;
}

.logo h1 {
  margin: 0;
  font-size: 1.75rem;
  font-weight: 600;
}

.nav-menu {
  display: flex;
  gap: 1rem;
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.25rem;
  background: rgba(255, 255, 255, 0.2);
  border-radius: 8px;
  color: white;
  text-decoration: none;
  transition: all 0.3s ease;
  font-weight: 500;
}

.nav-link:hover {
  background: rgba(255, 255, 255, 0.3);
  transform: translateY(-2px);
}

.user-menu {
  display: flex;
  align-items: center;
}

.user-menu .p-button {
  color: white;
  font-weight: 500;
}

.user-menu .p-button:hover {
  background: rgba(255, 255, 255, 0.1);
}

.app-main {
  flex: 1;
  padding: 2rem 0;
  background: #f8f9fa;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.app-footer {
  background: #2c3e50;
  color: white;
  padding: 1.5rem 0;
  text-align: center;
}

.app-footer p {
  margin: 0;
  opacity: 0.8;
}
</style>
