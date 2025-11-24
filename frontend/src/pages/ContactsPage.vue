<template>
  <div class="relative min-h-screen w-full overflow-hidden bg-background text-foreground">
    <!-- Background Pattern -->
    <div class="absolute inset-0 z-0">
      <div class="absolute inset-0 bg-[linear-gradient(to_right,#80808012_1px,transparent_1px),linear-gradient(to_bottom,#80808012_1px,transparent_1px)] bg-[size:24px_24px]"></div>
      <div class="absolute left-0 right-0 top-0 -z-10 m-auto h-[310px] w-[310px] rounded-full bg-primary/20 opacity-20 blur-[100px]"></div>
    </div>

    <div class="relative z-10 container mx-auto px-4 py-12">
      <!-- Header Section -->
      <div class="mb-12 flex flex-col items-center justify-between gap-6 md:flex-row">
        <div class="space-y-2 text-center md:text-left">
          <h1 class="text-4xl font-bold tracking-tighter sm:text-5xl md:text-6xl bg-gradient-to-b from-foreground to-foreground/70 bg-clip-text text-transparent">
            Contatos
          </h1>
          <p class="max-w-[600px] text-muted-foreground md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
            Gerencie sua rede de contatos com estilo e eficiência.
          </p>
        </div>
        
        <ShimmerButton @click="openCreateDialog">
          <span class="whitespace-pre-wrap text-center text-sm font-medium leading-none tracking-tight text-white dark:from-white dark:to-slate-900/10 lg:text-lg">
            Novo Contato
          </span>
        </ShimmerButton>
      </div>

      <!-- Loading State -->
      <div v-if="contactStore.loading" class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
        <div v-for="i in 6" :key="i" class="h-[200px] rounded-xl border bg-card/50 animate-pulse"></div>
      </div>

      <!-- Empty State -->
      <div v-else-if="contactStore.sortedContacts.length === 0" class="flex flex-col items-center justify-center py-20 text-center">
        <div class="rounded-full bg-muted p-6 mb-4">
          <i class="pi pi-users text-4xl text-muted-foreground"></i>
        </div>
        <h3 class="text-xl font-semibold">Nenhum contato encontrado</h3>
        <p class="text-muted-foreground mt-2 mb-6">Comece adicionando seu primeiro contato.</p>
        <Button label="Adicionar Contato" icon="pi pi-plus" @click="openCreateDialog" />
      </div>

      <!-- Contacts Grid -->
      <div v-else class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
        <ContactCard
          v-for="contact in contactStore.sortedContacts"
          :key="contact.id"
          :contact="contact"
          @edit="openEditDialog"
          @delete="confirmDelete"
        />
      </div>
    </div>

    <!-- Create/Edit Dialog -->
    <Dialog
      v-model:visible="dialogVisible"
      :header="dialogMode === 'create' ? 'Criar Novo Contato' : 'Editar Contato'"
      :modal="true"
      :closable="true"
      :draggable="false"
      class="contact-dialog p-0 overflow-hidden rounded-lg"
      :style="{ width: '500px' }"
      @hide="resetDialog"
      :pt="{
        root: { class: 'bg-slate-900/80 backdrop-blur-xl border border-white/10 shadow-2xl' },
        header: { class: 'bg-transparent border-b border-white/10 p-6' },
        content: { class: 'bg-transparent p-0' },
        title: { class: 'text-white font-semibold text-xl' },
        closeButton: { class: 'text-slate-400 hover:text-white transition-colors' },
        mask: { class: 'backdrop-blur-sm bg-black/50' }
      }"
    >
      <div class="p-6">
        <ContactForm
          :contact="selectedContact"
          :loading="contactStore.loading"
          @submit="handleSubmit"
          @cancel="closeDialog"
        />
      </div>
    </Dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useContactStore } from '@/store/contactStore'
import { useToast } from 'primevue/usetoast'
import { useConfirm } from 'primevue/useconfirm'
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import ShimmerButton from '@/components/ui/ShimmerButton.vue'
import ContactCard from '@/components/ui/ContactCard.vue'
import ContactForm from '@/components/ContactForm.vue'

const contactStore = useContactStore()
const toast = useToast()
const confirm = useConfirm()

const dialogVisible = ref(false)
const dialogMode = ref('create') // 'create' or 'edit'
const selectedContact = ref(null)

onMounted(() => {
  contactStore.fetchContacts()
})

const openCreateDialog = () => {
  dialogMode.value = 'create'
  selectedContact.value = null
  dialogVisible.value = true
}

const openEditDialog = (contact) => {
  dialogMode.value = 'edit'
  selectedContact.value = { ...contact }
  dialogVisible.value = true
}

const closeDialog = () => {
  dialogVisible.value = false
  resetDialog()
}

const resetDialog = () => {
  selectedContact.value = null
  dialogMode.value = 'create'
}

const handleSubmit = async (formData) => {
  try {
    if (dialogMode.value === 'create') {
      await contactStore.createContact(formData)
      toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Contato criado com sucesso', life: 3000 })
    } else {
      await contactStore.updateContact(selectedContact.value.id, formData)
      toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Contato atualizado com sucesso', life: 3000 })
    }
    closeDialog()
    contactStore.fetchContacts() // Refresh list
  } catch (error) {
    toast.add({ severity: 'error', summary: 'Erro', detail: error.message || 'Ocorreu um erro ao salvar', life: 3000 })
  }
}

const confirmDelete = (contact) => {
  confirm.require({
    message: `Tem certeza que deseja excluir ${contact.name}?`,
    header: 'Confirmar Exclusão',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    accept: async () => {
      try {
        await contactStore.deleteContact(contact.id)
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Contato excluído com sucesso', life: 3000 })
        contactStore.fetchContacts()
      } catch (error) {
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao excluir contato', life: 3000 })
      }
    }
  })
}
</script>

<style scoped>
/* Scoped styles if needed, but mostly using Tailwind now */
</style>
