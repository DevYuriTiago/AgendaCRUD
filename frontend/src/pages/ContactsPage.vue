<template>
  <div class="contacts-page fade-in">
    <div class="page-header">
      <div class="header-content">
        <div>
          <h1 class="page-title">Manage Your Contacts</h1>
          <p class="page-subtitle">Create, edit, and organize your contact list efficiently</p>
        </div>
        <Button
          label="New Contact"
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
      :header="dialogMode === 'create' ? 'Create New Contact' : 'Edit Contact'"
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
      summary: 'Error',
      detail: 'Failed to load contacts',
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
        summary: 'Success',
        detail: 'Contact created successfully',
        life: 3000
      })
    } else {
      await contactStore.updateContact(selectedContact.value.id, contactData)
      toast.add({
        severity: 'success',
        summary: 'Success',
        detail: 'Contact updated successfully',
        life: 3000
      })
    }
    closeDialog()
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: error.message || 'Operation failed',
      life: 5000
    })
  }
}

function confirmDelete(contact) {
  confirm.require({
    message: `Are you sure you want to delete "${contact.name}"?`,
    header: 'Confirm Deletion',
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
      summary: 'Success',
      detail: 'Contact deleted successfully',
      life: 3000
    })
  } catch (error) {
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: error.message || 'Failed to delete contact',
      life: 5000
    })
  }
}
</script>

<style scoped>
.contacts-page {
  animation: fadeIn 0.4s ease-in-out;
}

.page-header {
  margin-bottom: 2rem;
  padding: 2rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(0, 0, 0, 0.08);
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
  font-size: 2rem;
  font-weight: 700;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.page-subtitle {
  margin: 0;
  color: #6c757d;
  font-size: 1.1rem;
}

.create-button {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  padding: 1rem 2rem;
  font-size: 1rem;
  font-weight: 600;
  border-radius: 10px;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
}

.create-button:hover {
  transform: translateY(-3px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

:deep(.contact-dialog .p-dialog-header) {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5rem;
  border-radius: 12px 12px 0 0;
}

:deep(.contact-dialog .p-dialog-header-icon) {
  color: white;
}

:deep(.contact-dialog .p-dialog-header-icon:hover) {
  background: rgba(255, 255, 255, 0.2);
}

:deep(.contact-dialog .p-dialog-content) {
  padding: 0;
  border-radius: 0 0 12px 12px;
}

@media (max-width: 768px) {
  .page-header {
    padding: 1.5rem;
  }

  .header-content {
    flex-direction: column;
    align-items: stretch;
  }

  .create-button {
    width: 100%;
    justify-content: center;
  }

  .page-title {
    font-size: 1.5rem;
  }

  :deep(.contact-dialog) {
    width: 95vw !important;
  }
}
</style>
