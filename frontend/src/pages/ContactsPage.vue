<template>
  <div class="contacts-page fade-in">
    <div class="page-header">
      <div class="header-content">
        <div>
          <h1 class="page-title">Gerenciar Seus Contatos</h1>
          <p class="page-subtitle">Crie, edite e organize sua lista de contatos de forma eficiente</p>
        </div>
        <Button
          label="Novo Contato"
          icon="pi pi-plus"
          class="p-button-lg create-button"
          @click="openCreateDialog"
        />
      </div>
    </div>

    <ContactTable
      :contacts="contactStore.sortedContacts"
      :loading="contactStore.loading"
      @edit="openEditDialog"
      @delete="confirmDelete"
    />

    <!-- Create/Edit Dialog -->
    <Dialog
      v-model:visible="dialogVisible"
      :header="dialogMode === 'create' ? 'Criar Novo Contato' : 'Editar Contato'"
      :modal="true"
      :closable="true"
      :draggable="false"
      class="contact-dialog"
      :style="{ width: '600px' }"
      @hide="resetDialog"
    >
      <ContactForm
        :contact="selectedContact"
        :loading="contactStore.loading"
        @submit="handleSubmit"
        @cancel="closeDialog"
      />
    </Dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useContactStore } from '@/store/contactStore'
import { useToast } from 'primevue/usetoast'
import { useConfirm } from 'primevue/useconfirm'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import ContactTable from '@/components/ContactTable.vue'
import ContactForm from '@/components/ContactForm.vue'

const contactStore = useContactStore()
const toast = useToast()
const confirm = useConfirm()

const dialogVisible = ref(false)
const dialogMode = ref('create') // 'create' or 'edit'
const selectedContact = ref(null)

onMounted(async () => {
  try {
    await contactStore.fetchContacts()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: 'Erro',
      detail: 'Falha ao carregar contatos',
      life: 3000
    })
  }
})

function openCreateDialog() {
  dialogMode.value = 'create'
  selectedContact.value = null
  dialogVisible.value = true
}

function openEditDialog(contact) {
  dialogMode.value = 'edit'
  selectedContact.value = { ...contact }
  dialogVisible.value = true
}

function closeDialog() {
  dialogVisible.value = false
  resetDialog()
}

function resetDialog() {
  selectedContact.value = null
  dialogMode.value = 'create'
}

async function handleSubmit(contactData) {
  try {
    if (dialogMode.value === 'create') {
      await contactStore.createContact(contactData)
      toast.add({
        severity: 'success',
        summary: 'Sucesso',
        detail: 'Contato criado com sucesso',
        life: 3000
      })
    } else {
      await contactStore.updateContact(selectedContact.value.id, contactData)
      toast.add({
        severity: 'success',
        summary: 'Sucesso',
        detail: 'Contato atualizado com sucesso',
        life: 3000
      })
    }
    closeDialog()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: 'Erro',
      detail: error.message || 'Operação falhou',
      life: 5000
    })
  }
}

function confirmDelete(contact) {
  confirm.require({
    message: `Tem certeza que deseja excluir "${contact.name}"?`,
    header: 'Confirmar Exclusão',
    icon: 'pi pi-exclamation-triangle',
    acceptClass: 'p-button-danger',
    accept: () => handleDelete(contact.id)
  })
}

async function handleDelete(contactId) {
  try {
    await contactStore.deleteContact(contactId)
    toast.add({
      severity: 'success',
      summary: 'Sucesso',
      detail: 'Contato excluído com sucesso',
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: 'Erro',
      detail: error.message || 'Falha ao excluir contato',
      life: 5000
    })
  }
}
</script>

<style scoped>
.contacts-page {
  animation: fadeIn 0.5s cubic-bezier(0.16, 1, 0.3, 1);
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #e3e7f0 100%);
  padding: 2rem;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.page-header {
  margin-bottom: 2rem;
  padding: 2.5rem;
  background: linear-gradient(135deg, rgba(255,255,255,0.95) 0%, rgba(255,255,255,0.9) 100%);
  backdrop-filter: blur(20px);
  border-radius: 20px;
  box-shadow: 0 10px 40px rgba(102, 126, 234, 0.15), 0 0 1px rgba(255,255,255,0.5) inset;
  border: 1px solid rgba(255, 255, 255, 0.5);
  position: relative;
  overflow: hidden;
}

.page-header::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #667eea 0%, #764ba2 100%);
}

.header-content {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 2rem;
  flex-wrap: wrap;
}

.page-title {
  margin: 0 0 0.5rem 0;
  font-size: 2.25rem;
  font-weight: 800;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  letter-spacing: -0.5px;
}

.page-subtitle {
  margin: 0;
  color: #64748b;
  font-size: 1.1rem;
  font-weight: 500;
}

.create-button {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  padding: 1rem 2rem;
  font-size: 1rem;
  font-weight: 600;
  border-radius: 12px;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 4px 20px rgba(102, 126, 234, 0.35);
  position: relative;
  overflow: hidden;
}

.create-button::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
  transition: left 0.5s;
}

.create-button:hover::before {
  left: 100%;
}

.create-button:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 30px rgba(102, 126, 234, 0.5);
}

.create-button:active {
  transform: translateY(-1px);
}

:deep(.contact-dialog .p-dialog) {
  border-radius: 20px;
  overflow: hidden;
  box-shadow: 0 25px 80px rgba(0, 0, 0, 0.3);
  border: 1px solid rgba(255, 255, 255, 0.3);
}

:deep(.contact-dialog .p-dialog-header) {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.75rem 2rem;
  border-radius: 0;
  border-bottom: none;
}

:deep(.contact-dialog .p-dialog-title) {
  font-weight: 700;
  font-size: 1.5rem;
  letter-spacing: -0.3px;
}

:deep(.contact-dialog .p-dialog-header-icon) {
  color: white;
  width: 2.5rem;
  height: 2.5rem;
  border-radius: 50%;
  transition: all 0.2s ease;
}

:deep(.contact-dialog .p-dialog-header-icon:hover) {
  background: rgba(255, 255, 255, 0.2);
  transform: rotate(90deg);
}

:deep(.contact-dialog .p-dialog-content) {
  padding: 0;
  border-radius: 0;
  background: #fff;
}

@media (max-width: 768px) {
  .contacts-page {
    padding: 1rem;
  }

  .page-header {
    padding: 1.5rem;
    margin-bottom: 1.5rem;
  }

  .header-content {
    flex-direction: column;
    align-items: stretch;
    gap: 1.5rem;
  }

  .create-button {
    width: 100%;
    justify-content: center;
  }

  .page-title {
    font-size: 1.75rem;
  }

  .page-subtitle {
    font-size: 1rem;
  }

  :deep(.contact-dialog) {
    width: 95vw !important;
    max-width: 95vw !important;
  }

  :deep(.contact-dialog .p-dialog-header) {
    padding: 1.25rem 1.5rem;
  }

  :deep(.contact-dialog .p-dialog-title) {
    font-size: 1.25rem;
  }
}

@media (max-width: 480px) {
  .contacts-page {
    padding: 0.75rem;
  }

  .page-header {
    padding: 1.25rem;
    margin-bottom: 1rem;
  }

  .page-title {
    font-size: 1.5rem;
  }

  .page-subtitle {
    font-size: 0.9rem;
  }

  .create-button {
    padding: 0.875rem 1.5rem;
    font-size: 0.95rem;
  }

  :deep(.contact-dialog .p-dialog-header) {
    padding: 1rem 1.25rem;
  }

  :deep(.contact-dialog .p-dialog-title) {
    font-size: 1.1rem;
  }
}
</style>
