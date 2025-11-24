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
              <h1>Agenda de Contatos</h1>
            </div>
            <nav class="nav-menu">
              <router-link to="/contacts" class="nav-link">
                <i class="pi pi-th-large"></i>
                <span>Contatos</span>
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
          <p>Agenda de Contatos. Feito com .NET + VUE</p>
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
    label: 'Perfil',
    icon: 'pi pi-user',
    command: () => {
      toast.add({ severity: 'info', summary: 'Perfil', detail: 'Funcionalidade em breve', life: 3000 })
    }
  },
  {
    separator: true
  },
  {
    label: 'Sair',
    icon: 'pi pi-sign-out',
    command: () => {
      confirm.require({
        message: 'Tem certeza que deseja sair?',
        header: 'Confirmar Saída',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          authStore.logout()
          toast.add({ severity: 'success', summary: 'Desconectado', detail: 'Até logo!', life: 3000 })
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
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  box-shadow: 0 4px 30px rgba(0, 0, 0, 0.08);
  padding: 1.25rem 0;
  position: sticky;
  top: 0;
  z-index: 1000;
  border-bottom: 1px solid rgba(102, 126, 234, 0.1);
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
  animation: slideInLeft 0.6s ease-out;
}

@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.logo i {
  font-size: 2.25rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  filter: drop-shadow(0 2px 8px rgba(102, 126, 234, 0.3));
}

.logo h1 {
  margin: 0;
  font-size: 1.75rem;
  font-weight: 800;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  letter-spacing: -0.5px;
}

.nav-menu {
  display: flex;
  gap: 1rem;
  align-items: center;
  animation: slideInRight 0.6s ease-out;
}

@keyframes slideInRight {
  from {
    opacity: 0;
    transform: translateX(30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 0.625rem;
  padding: 0.75rem 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 12px;
  color: white;
  text-decoration: none;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  font-weight: 600;
  font-size: 0.95rem;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
  position: relative;
  overflow: hidden;
}

.nav-link::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
  transition: left 0.5s;
}

.nav-link:hover::before {
  left: 100%;
}

.nav-link:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 25px rgba(102, 126, 234, 0.45);
}

.user-menu {
  display: flex;
  align-items: center;
}

:deep(.user-menu .p-button) {
  color: #334155;
  font-weight: 600;
  padding: 0.75rem 1.25rem;
  border-radius: 12px;
  transition: all 0.3s ease;
}

:deep(.user-menu .p-button:hover) {
  background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
  color: #667eea;
}

.app-main {
  flex: 1;
  padding: 0;
}

.container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 2rem;
}

.app-footer {
  background: linear-gradient(135deg, #1e293b 0%, #334155 100%);
  color: white;
  padding: 2rem 0;
  text-align: center;
  border-top: 3px solid #667eea;
}

.app-footer p {
  margin: 0;
  opacity: 0.9;
  font-weight: 500;
  font-size: 0.95rem;
}

@media (max-width: 768px) {
  .logo h1 {
    font-size: 1.25rem;
  }

  .nav-menu {
    gap: 0.5rem;
  }

  .nav-link span {
    display: none;
  }

  .container {
    padding: 0 1rem;
  }
}
</style>

<style>
/* Modern ConfirmDialog Styles */
.p-confirm-dialog {
  background: rgba(255, 255, 255, 0.98) !important;
  backdrop-filter: blur(30px) !important;
  border-radius: 24px !important;
  border: 1px solid rgba(102, 126, 234, 0.15) !important;
  box-shadow: 0 25px 50px rgba(0, 0, 0, 0.15), 
              0 10px 20px rgba(102, 126, 234, 0.1) !important;
  overflow: hidden !important;
  animation: dialogZoomIn 0.3s cubic-bezier(0.34, 1.56, 0.64, 1) !important;
}

@keyframes dialogZoomIn {
  from {
    opacity: 0;
    transform: scale(0.9) translateY(-20px);
  }
  to {
    opacity: 1;
    transform: scale(1) translateY(0);
  }
}

.p-confirm-dialog .p-dialog-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%) !important;
  padding: 1.75rem 2rem !important;
  border-bottom: none !important;
  position: relative !important;
  overflow: hidden !important;
}

.p-confirm-dialog .p-dialog-header::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  animation: shimmer 3s infinite;
}

@keyframes shimmer {
  0%, 100% {
    left: -100%;
  }
  50% {
    left: 100%;
  }
}

.p-confirm-dialog .p-dialog-title {
  color: white !important;
  font-weight: 700 !important;
  font-size: 1.35rem !important;
  letter-spacing: -0.3px !important;
  text-shadow: 0 2px 8px rgba(0, 0, 0, 0.15) !important;
}

.p-confirm-dialog .p-dialog-header-icon {
  color: white !important;
  width: 2.5rem !important;
  height: 2.5rem !important;
  border-radius: 12px !important;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1) !important;
  background: rgba(255, 255, 255, 0.15) !important;
  backdrop-filter: blur(10px) !important;
}

.p-confirm-dialog .p-dialog-header-icon:hover {
  background: rgba(255, 255, 255, 0.25) !important;
  transform: rotate(90deg) scale(1.1) !important;
}

.p-confirm-dialog .p-dialog-content {
  padding: 2.5rem 2rem !important;
  background: linear-gradient(180deg, #ffffff 0%, #f8fafc 100%) !important;
}

.p-confirm-dialog .p-confirm-dialog-icon {
  font-size: 3.5rem !important;
  color: #f59e0b !important;
  margin-right: 1.5rem !important;
  filter: drop-shadow(0 4px 12px rgba(245, 158, 11, 0.3)) !important;
  animation: iconPulse 2s ease-in-out infinite !important;
}

@keyframes iconPulse {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
}

.p-confirm-dialog .p-confirm-dialog-message {
  font-size: 1.125rem !important;
  color: #334155 !important;
  line-height: 1.7 !important;
  font-weight: 500 !important;
}

.p-confirm-dialog .p-dialog-footer {
  padding: 1.5rem 2rem 2rem !important;
  background: linear-gradient(180deg, #f8fafc 0%, #f1f5f9 100%) !important;
  border-top: 1px solid rgba(102, 126, 234, 0.1) !important;
  display: flex !important;
  gap: 1rem !important;
}

.p-confirm-dialog .p-dialog-footer button {
  flex: 1 !important;
  padding: 0.875rem 2rem !important;
  font-weight: 600 !important;
  font-size: 1rem !important;
  border-radius: 12px !important;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1) !important;
  position: relative !important;
  overflow: hidden !important;
}

.p-confirm-dialog .p-dialog-footer button::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 0;
  height: 0;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.4);
  transform: translate(-50%, -50%);
  transition: width 0.6s, height 0.6s;
}

.p-confirm-dialog .p-dialog-footer button:hover::before {
  width: 300px;
  height: 300px;
}

.p-confirm-dialog .p-button-danger {
  background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%) !important;
  border: none !important;
  box-shadow: 0 4px 15px rgba(239, 68, 68, 0.3) !important;
}

.p-confirm-dialog .p-button-danger:hover {
  transform: translateY(-2px) !important;
  box-shadow: 0 6px 25px rgba(239, 68, 68, 0.45) !important;
}

.p-confirm-dialog .p-button-secondary {
  background: linear-gradient(135deg, #64748b 0%, #475569 100%) !important;
  border: none !important;
  box-shadow: 0 4px 15px rgba(100, 116, 139, 0.3) !important;
}

.p-confirm-dialog .p-button-secondary:hover {
  transform: translateY(-2px) !important;
  box-shadow: 0 6px 25px rgba(100, 116, 139, 0.45) !important;
}

/* Modern Menu Styles */
.p-menu {
  background: rgba(255, 255, 255, 0.98) !important;
  backdrop-filter: blur(30px) !important;
  border-radius: 16px !important;
  border: 1px solid rgba(102, 126, 234, 0.15) !important;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.12), 
              0 8px 16px rgba(102, 126, 234, 0.08) !important;
  padding: 0.75rem !important;
  min-width: 220px !important;
  animation: menuSlideDown 0.3s cubic-bezier(0.34, 1.56, 0.64, 1) !important;
}

@keyframes menuSlideDown {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.p-menu .p-menuitem-link {
  border-radius: 10px !important;
  padding: 0.875rem 1.125rem !important;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1) !important;
  color: #334155 !important;
  font-weight: 500 !important;
}

.p-menu .p-menuitem-link:hover {
  background: linear-gradient(135deg, #f0f4ff 0%, #e9efff 100%) !important;
  color: #667eea !important;
  transform: translateX(4px) !important;
}

.p-menu .p-menuitem-icon {
  color: #667eea !important;
  font-size: 1.125rem !important;
  margin-right: 0.875rem !important;
  transition: transform 0.3s ease !important;
}

.p-menu .p-menuitem-link:hover .p-menuitem-icon {
  transform: scale(1.15) !important;
}

.p-menu .p-menuitem-separator {
  border-top: 1px solid rgba(102, 126, 234, 0.15) !important;
  margin: 0.5rem 0 !important;
}

/* Modern Toast Styles */
.p-toast {
  opacity: 1 !important;
}

.p-toast-message {
  background: rgba(255, 255, 255, 0.98) !important;
  backdrop-filter: blur(30px) !important;
  border-radius: 16px !important;
  border: 1px solid rgba(102, 126, 234, 0.15) !important;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.12), 
              0 8px 16px rgba(102, 126, 234, 0.08) !important;
  animation: toastSlideIn 0.4s cubic-bezier(0.34, 1.56, 0.64, 1) !important;
}

@keyframes toastSlideIn {
  from {
    opacity: 0;
    transform: translateX(100px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.p-toast-message-content {
  padding: 1.25rem !important;
}

.p-toast-message-icon {
  font-size: 2rem !important;
}

.p-toast-message-text {
  margin-left: 1.25rem !important;
}

.p-toast-summary {
  font-weight: 700 !important;
  font-size: 1.05rem !important;
  color: #1e293b !important;
}

.p-toast-detail {
  margin-top: 0.5rem !important;
  color: #475569 !important;
  font-weight: 500 !important;
}

.p-toast-message-success .p-toast-message-icon,
.p-toast-message-success .p-toast-icon-close {
  color: #10b981 !important;
}

.p-toast-message-info .p-toast-message-icon,
.p-toast-message-info .p-toast-icon-close {
  color: #3b82f6 !important;
}

.p-toast-message-warn .p-toast-message-icon,
.p-toast-message-warn .p-toast-icon-close {
  color: #f59e0b !important;
}

.p-toast-message-error .p-toast-message-icon,
.p-toast-message-error .p-toast-icon-close {
  color: #ef4444 !important;
}

.p-toast-icon-close {
  border-radius: 8px !important;
  transition: all 0.3s ease !important;
}

.p-toast-icon-close:hover {
  background: rgba(102, 126, 234, 0.1) !important;
  transform: rotate(90deg) !important;
}

/* Dialog Overlay */
.p-dialog-mask {
  backdrop-filter: blur(8px) !important;
  background: rgba(0, 0, 0, 0.5) !important;
  animation: overlayFadeIn 0.3s ease-out !important;
}

@keyframes overlayFadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

/* Modern Dropdown Panel Styles */
.p-dropdown-panel {
  background: rgba(255, 255, 255, 0.98) !important;
  backdrop-filter: blur(30px) !important;
  border-radius: 16px !important;
  border: 1px solid rgba(102, 126, 234, 0.15) !important;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.12), 
              0 8px 16px rgba(102, 126, 234, 0.08) !important;
  padding: 0.75rem !important;
  animation: dropdownSlideDown 0.3s cubic-bezier(0.34, 1.56, 0.64, 1) !important;
}

@keyframes dropdownSlideDown {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.p-dropdown-panel .p-dropdown-items {
  padding: 0 !important;
}

.p-dropdown-panel .p-dropdown-item {
  border-radius: 10px !important;
  padding: 0.875rem 1.125rem !important;
  margin: 0.25rem 0 !important;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1) !important;
  color: #334155 !important;
  font-weight: 600 !important;
  font-size: 1rem !important;
}

.p-dropdown-panel .p-dropdown-item:hover {
  background: linear-gradient(135deg, #f0f4ff 0%, #e9efff 100%) !important;
  color: #667eea !important;
  transform: translateX(4px) !important;
}

.p-dropdown-panel .p-dropdown-item.p-highlight {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%) !important;
  color: white !important;
}

.p-dropdown-panel .p-dropdown-item.p-focus {
  box-shadow: none !important;
}

/* Tooltip Styles */
.p-tooltip {
  background: rgba(30, 41, 59, 0.95) !important;
  backdrop-filter: blur(20px) !important;
  border-radius: 10px !important;
  padding: 0.625rem 1rem !important;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2) !important;
  animation: tooltipFadeIn 0.2s ease-out !important;
}

@keyframes tooltipFadeIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}

.p-tooltip .p-tooltip-text {
  font-size: 0.875rem !important;
  font-weight: 600 !important;
  letter-spacing: 0.3px !important;
}

.p-tooltip .p-tooltip-arrow {
  border-top-color: rgba(30, 41, 59, 0.95) !important;
}
</style>
