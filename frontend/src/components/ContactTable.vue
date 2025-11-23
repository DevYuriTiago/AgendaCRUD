<template>
  <div class="contact-table-wrapper">
    <DataTable
      :value="contacts"
      :loading="loading"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 25, 50]"
      stripedRows
      showGridlines
      dataKey="id"
      filterDisplay="row"
      v-model:filters="filters"
      :globalFilterFields="['name', 'email', 'phone']"
      responsiveLayout="scroll"
      class="contact-table"
    >
      <template #header>
        <div class="table-header">
          <h2 class="table-title">
            <i class="pi pi-users"></i>
            Contacts ({{ contacts.length }})
          </h2>
          <span class="p-input-icon-left search-box">
            <i class="pi pi-search" />
            <InputText
              v-model="filters['global'].value"
              placeholder="Search contacts..."
              class="search-input"
            />
          </span>
        </div>
      </template>

      <template #empty>
        <div class="empty-state">
          <i class="pi pi-inbox empty-icon"></i>
          <p>No contacts found</p>
          <small>Create your first contact to get started</small>
        </div>
      </template>

      <Column field="name" header="Name" :sortable="true" style="min-width: 200px">
        <template #body="{ data }">
          <div class="name-cell">
            <Avatar
              :label="data.name.charAt(0).toUpperCase()"
              class="contact-avatar"
              shape="circle"
            />
            <span class="contact-name">{{ data.name }}</span>
          </div>
        </template>
      </Column>

      <Column field="email" header="Email" :sortable="true" style="min-width: 250px">
        <template #body="{ data }">
          <div class="email-cell">
            <i class="pi pi-envelope"></i>
            <a :href="`mailto:${data.email}`" class="email-link">{{ data.email }}</a>
          </div>
        </template>
      </Column>

      <Column field="phone" header="Phone" :sortable="true" style="min-width: 180px">
        <template #body="{ data }">
          <div class="phone-cell">
            <i class="pi pi-phone"></i>
            <span>{{ formatPhone(data.phone) }}</span>
          </div>
        </template>
      </Column>

      <Column field="createdAt" header="Created" :sortable="true" style="min-width: 180px">
        <template #body="{ data }">
          <Tag :value="formatDate(data.createdAt)" severity="info" />
        </template>
      </Column>

      <Column header="Actions" :exportable="false" style="min-width: 150px">
        <template #body="{ data }">
          <div class="action-buttons">
            <Button
              icon="pi pi-pencil"
              class="p-button-rounded p-button-text p-button-warning"
              @click="$emit('edit', data)"
              v-tooltip.top="'Edit'"
            />
            <Button
              icon="pi pi-trash"
              class="p-button-rounded p-button-text p-button-danger"
              @click="$emit('delete', data)"
              v-tooltip.top="'Delete'"
            />
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { FilterMatchMode } from 'primevue/api'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import Avatar from 'primevue/avatar'
import Tag from 'primevue/tag'

defineProps({
  contacts: {
    type: Array,
    default: () => []
  },
  loading: {
    type: Boolean,
    default: false
  }
})

defineEmits(['edit', 'delete'])

const filters = ref({
  global: { value: null, matchMode: FilterMatchMode.CONTAINS }
})

function formatPhone(phone) {
  if (!phone) return ''
  // Format: (XX) XXXXX-XXXX
  if (phone.length === 11) {
    return `(${phone.substr(0, 2)}) ${phone.substr(2, 5)}-${phone.substr(7)}`
  }
  return phone
}

function formatDate(dateString) {
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', { 
    year: 'numeric', 
    month: 'short', 
    day: 'numeric' 
  })
}
</script>

<style scoped>
.contact-table-wrapper {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  overflow: hidden;
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  flex-wrap: wrap;
  gap: 1rem;
}

.table-title {
  margin: 0;
  font-size: 1.5rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.search-box {
  flex: 1;
  max-width: 400px;
  min-width: 250px;
}

.search-input {
  width: 100%;
  padding: 0.75rem 1rem 0.75rem 2.5rem;
  border-radius: 8px;
  border: none;
  background: rgba(255, 255, 255, 0.95);
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  color: #6c757d;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
  opacity: 0.4;
}

.name-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.contact-avatar {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  font-weight: 600;
}

.contact-name {
  font-weight: 500;
  color: #2c3e50;
}

.email-cell,
.phone-cell {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #495057;
}

.email-cell i,
.phone-cell i {
  color: #667eea;
}

.email-link {
  color: #667eea;
  text-decoration: none;
  transition: color 0.2s;
}

.email-link:hover {
  color: #764ba2;
  text-decoration: underline;
}

.action-buttons {
  display: flex;
  gap: 0.25rem;
}

:deep(.p-datatable) {
  border: none;
}

:deep(.p-datatable .p-datatable-thead > tr > th) {
  background: #f8f9fa;
  color: #495057;
  font-weight: 600;
  border-bottom: 2px solid #dee2e6;
}

:deep(.p-datatable .p-datatable-tbody > tr) {
  transition: all 0.2s;
}

:deep(.p-datatable .p-datatable-tbody > tr:hover) {
  background: #f8f9fa;
  transform: translateX(4px);
}
</style>
