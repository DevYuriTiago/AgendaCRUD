<template>
  <div id="app" class="min-h-screen bg-background font-sans antialiased">
    <Toast />
    <ConfirmDialog 
      :pt="{
        root: { class: 'bg-slate-900/80 backdrop-blur-xl border border-white/10 shadow-2xl w-[400px]' },
        header: { class: 'bg-transparent border-b border-white/10 p-6' },
        content: { class: 'bg-transparent p-0 pl-6 pr-6 pt-6' },
        footer: { class: 'bg-transparent p-6 border-t border-white/10 gap-2 flex justify-end' },
        title: { class: 'text-white font-semibold text-xl' },
        closeButton: { class: 'text-slate-400 hover:text-white transition-colors' },
        icon: { class: 'text-blue-400 text-2xl mr-4' },
        message: { class: 'text-slate-300' },
        mask: { class: 'backdrop-blur-sm bg-black/50' }
      }"
    />
    
    <div class="relative flex min-h-screen flex-col">
      <header v-if="authStore.isAuthenticated" class="sticky top-0 z-50 w-full border-b border-border/40 bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
        <div class="container flex h-14 max-w-screen-2xl items-center">
          <div class="mr-4 flex">
            <a class="mr-6 flex items-center space-x-2" href="/">
              <i class="pi pi-users text-primary"></i>
              <span class="hidden font-bold sm:inline-block">Agenda</span>
            </a>
            <nav class="flex items-center space-x-6 text-sm font-medium">
              <router-link to="/contacts" class="transition-colors hover:text-foreground/80 text-foreground/60" active-class="text-foreground">
                Contatos
              </router-link>
            </nav>
          </div>
          <div class="flex flex-1 items-center justify-between space-x-2 md:justify-end">
            <div class="w-full flex-1 md:w-auto md:flex-none">
              <!-- Search could go here -->
            </div>
            <div class="flex items-center gap-2">
              <Button
                :label="authStore.user?.fullName"
                icon="pi pi-user"
                text
                class="text-sm font-medium"
                @click="toggleMenu"
                aria-haspopup="true"
                aria-controls="overlay_menu"
              />
              <Menu ref="menu" id="overlay_menu" :model="menuItems" :popup="true" />
            </div>
          </div>
        </div>
      </header>

      <main class="flex-1">
        <router-view />
      </main>

      <footer v-if="authStore.isAuthenticated" class="py-6 md:px-8 md:py-0">
        <div class="container flex flex-col items-center justify-between gap-4 md:h-24 md:flex-row">
          <p class="text-balance text-center text-sm leading-loose text-muted-foreground md:text-left">
            Agenda de Contatos. Feito com .NET + Vue.
          </p>
        </div>
      </footer>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/store/authStore'
import Toast from 'primevue/toast'
import ConfirmDialog from 'primevue/confirmdialog'
import Button from 'primevue/button'
import Menu from 'primevue/menu'

const router = useRouter()
const authStore = useAuthStore()
const menu = ref(null)

const menuItems = ref([
  {
    label: 'Perfil',
    icon: 'pi pi-user',
    command: () => {
      // router.push('/profile')
    }
  },
  {
    separator: true
  },
  {
    label: 'Sair',
    icon: 'pi pi-sign-out',
    command: () => {
      authStore.logout()
      router.push('/login')
    }
  }
])

const toggleMenu = (event) => {
  menu.value.toggle(event)
}
</script>

<style>
/* Global styles that might be needed for PrimeVue overrides if necessary */
</style>
