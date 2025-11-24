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
            Contatos ({{ contacts.length }})
          </h2>
          <span class="p-input-icon-left search-box">
            <i class="pi pi-search" />
            <InputText
              v-model="filters['global'].value"
              placeholder="Pesquisar contatos..."
              class="search-input"
            />
          </span>
        </div>
      </template>

      <template #empty>
        <div class="empty-state">
          <i class="pi pi-inbox empty-icon"></i>
          <p>Nenhum contato encontrado</p>
          <small>Crie seu primeiro contato para começar</small>
        </div>
      </template>

      <Column field="name" header="Nome" :sortable="true" style="min-width: 200px">
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

      <Column field="email" header="E-mail" :sortable="true" style="min-width: 250px">
        <template #body="{ data }">
          <div class="email-cell">
            <i class="pi pi-envelope"></i>
            <a :href="`mailto:${data.email}`" class="email-link">{{ data.email }}</a>
          </div>
        </template>
      </Column>

      <Column field="phone" header="Telefone" :sortable="true" style="min-width: 180px">
        <template #body="{ data }">
          <div class="phone-cell">
            <i class="pi pi-phone"></i>
            <span>{{ formatPhone(data.phone) }}</span>
          </div>
        </template>
      </Column>

      <Column field="createdAt" header="Criado em" :sortable="true" style="min-width: 180px">
        <template #body="{ data }">
          <Tag :value="formatDate(data.createdAt)" severity="info" />
        </template>
      </Column>

      <Column header="Ações" :exportable="false" style="min-width: 150px">
        <template #body="{ data }">
          <div class="action-buttons">
            <Button
              icon="pi pi-pencil"
              class="p-button-rounded action-edit"
              @click="$emit('edit', data)"
              v-tooltip.top="'Editar'"
            />
            <Button
              icon="pi pi-trash"
              class="p-button-rounded action-delete"
              @click="$emit('delete', data)"
              v-tooltip.top="'Excluir'"
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
  return date.toLocaleDateString('pt-BR', { 
    year: 'numeric', 
    month: 'short', 
    day: 'numeric' 
  })
}
</script>

<style scoped>
.contact-table-wrapper {
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(20px);
  border-radius: 20px;
  box-shadow: 0 10px 40px rgba(102, 126, 234, 0.15), 0 0 1px rgba(255,255,255,0.5) inset;
  overflow: hidden;
  border: 1px solid rgba(255, 255, 255, 0.5);
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 2rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  flex-wrap: wrap;
  gap: 1.5rem;
  position: relative;
  overflow: hidden;
}

.table-header::before {
  content: '';
  position: absolute;
  width: 200px;
  height: 200px;
  background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
  border-radius: 50%;
  top: -50px;
  right: -50px;
  animation: pulse 4s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { transform: scale(1); opacity: 0.5; }
  50% { transform: scale(1.1); opacity: 0.8; }
}

.table-title {
  margin: 0;
  font-size: 1.75rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  letter-spacing: -0.3px;
  text-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.search-box {
  flex: 1;
  max-width: 400px;
  min-width: 250px;
  position: relative;
  z-index: 1;
}

.search-input {
  width: 100%;
  padding: 0.875rem 1rem 0.875rem 2.75rem;
  border-radius: 12px;
  border: 2px solid transparent;
  background: rgba(255, 255, 255, 0.95);
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  font-size: 0.95rem;
}

.search-input:focus {
  background: white;
  border-color: rgba(255, 255, 255, 0.5);
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
  transform: translateY(-1px);
}

.empty-state {
  text-align: center;
  padding: 5rem 2rem;
  color: #64748b;
}

.empty-icon {
  font-size: 5rem;
  margin-bottom: 1.5rem;
  opacity: 0.3;
  animation: float 3s ease-in-out infinite;
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

.empty-state p {
  font-size: 1.25rem;
  font-weight: 600;
  margin: 0 0 0.5rem 0;
  color: #334155;
}

.empty-state small {
  font-size: 1rem;
  color: #94a3b8;
}

.name-cell {
  display: flex;
  align-items: center;
  gap: 0.875rem;
}

.contact-avatar {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  font-weight: 700;
  font-size: 1.1rem;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
  transition: all 0.3s ease;
}

.name-cell:hover .contact-avatar {
  transform: scale(1.1) rotate(5deg);
  box-shadow: 0 6px 16px rgba(102, 126, 234, 0.4);
}

.contact-name {
  font-weight: 600;
  color: #1e293b;
  font-size: 1rem;
}

.email-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.email-cell i {
  color: #667eea;
  font-size: 1.1rem;
}

.email-link {
  color: #475569;
  text-decoration: none;
  transition: all 0.2s ease;
  font-weight: 500;
}

.email-link:hover {
  color: #667eea;
  text-decoration: underline;
}

.phone-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  color: #475569;
  font-weight: 500;
}

.phone-cell i {
  color: #764ba2;
  font-size: 1.1rem;
}

.action-buttons {
  display: flex;
  gap: 0.75rem;
  justify-content: center;
}

.action-edit,
.action-delete {
  width: 2.75rem;
  height: 2.75rem;
  border: none;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  position: relative;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.action-edit::before,
.action-delete::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 0;
  height: 0;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.4);
  transform: translate(-50%, -50%);
  transition: width 0.4s, height 0.4s;
}

.action-edit:hover::before,
.action-delete:hover::before {
  width: 100px;
  height: 100px;
}

.action-edit {
  background: linear-gradient(135deg, #f59e0b 0%, #f97316 100%);
  color: white;
}

.action-edit:hover {
  transform: translateY(-3px) rotate(8deg);
  box-shadow: 0 8px 20px rgba(245, 158, 11, 0.5);
}

.action-delete {
  background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);
  color: white;
}

.action-delete:hover {
  transform: translateY(-3px) rotate(-8deg);
  box-shadow: 0 8px 20px rgba(239, 68, 68, 0.5);
}

:deep(.p-button-rounded) {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

:deep(.p-button-warning:hover) {
  background: #fbbf24 !important;
  box-shadow: 0 4px 12px rgba(251, 191, 36, 0.4);
}

:deep(.p-button-danger:hover) {
  background: #ef4444 !important;
  box-shadow: 0 4px 12px rgba(239, 68, 68, 0.4);
}

:deep(.p-datatable) {
  border: none;
}

:deep(.p-datatable .p-datatable-thead > tr > th) {
  background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
  color: #334155;
  font-weight: 700;
  border-bottom: 2px solid #e2e8f0;
  padding: 1.25rem 1rem;
  font-size: 0.95rem;
  letter-spacing: 0.3px;
  text-transform: uppercase;
}

:deep(.p-datatable .p-datatable-tbody > tr) {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  border-bottom: 1px solid #f1f5f9;
}

:deep(.p-datatable .p-datatable-tbody > tr:hover) {
  background: linear-gradient(135deg, #f8fafc 0%, #fff 100%);
  transform: translateX(8px);
  box-shadow: -4px 0 0 0 #667eea;
}

:deep(.p-datatable .p-datatable-tbody > tr > td) {
  padding: 1.25rem 1rem;
}

:deep(.p-tag) {
  padding: 0.5rem 1rem;
  border-radius: 8px;
  font-weight: 600;
  font-size: 0.85rem;
}

:deep(.p-paginator) {
  background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
  border-top: 1px solid #e2e8f0;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
}

:deep(.p-paginator .p-paginator-first),
:deep(.p-paginator .p-paginator-prev),
:deep(.p-paginator .p-paginator-next),
:deep(.p-paginator .p-paginator-last) {
  min-width: 2.75rem;
  height: 2.75rem;
  border-radius: 12px;
  background: white;
  border: 2px solid #e2e8f0;
  color: #667eea;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

:deep(.p-paginator .p-paginator-first:not(.p-disabled):hover),
:deep(.p-paginator .p-paginator-prev:not(.p-disabled):hover),
:deep(.p-paginator .p-paginator-next:not(.p-disabled):hover),
:deep(.p-paginator .p-paginator-last:not(.p-disabled):hover) {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-color: transparent;
  color: white;
  transform: translateY(-2px);
  box-shadow: 0 6px 16px rgba(102, 126, 234, 0.4);
}

:deep(.p-paginator .p-paginator-pages) {
  display: flex;
  gap: 0.5rem;
}

:deep(.p-paginator .p-paginator-pages .p-paginator-page) {
  min-width: 2.75rem;
  height: 2.75rem;
  border-radius: 12px;
  background: white;
  border: 2px solid #e2e8f0;
  color: #475569;
  font-weight: 600;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

:deep(.p-paginator .p-paginator-pages .p-paginator-page:hover) {
  background: linear-gradient(135deg, #f0f4ff 0%, #e9efff 100%);
  border-color: #667eea;
  color: #667eea;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.2);
}

:deep(.p-paginator .p-paginator-pages .p-paginator-page.p-highlight) {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-color: transparent;
  color: white;
  box-shadow: 0 6px 16px rgba(102, 126, 234, 0.4);
  transform: scale(1.1);
}

:deep(.p-paginator .p-dropdown) {
  border-radius: 12px;
  border: 2px solid #e2e8f0;
  background: white;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
  min-width: 4.5rem;
}

:deep(.p-paginator .p-dropdown:hover) {
  border-color: #667eea;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.2);
  transform: translateY(-1px);
}

:deep(.p-paginator .p-dropdown:focus),
:deep(.p-paginator .p-dropdown.p-focus) {
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.15);
}

:deep(.p-paginator .p-dropdown .p-dropdown-label) {
  padding: 0.625rem 1rem;
  font-weight: 600;
  color: #475569;
}

:deep(.p-paginator .p-dropdown .p-dropdown-trigger) {
  color: #667eea;
}

/* Responsividade completa */
@media (max-width: 768px) {
  .table-header {
    flex-direction: column;
    padding: 1.5rem;
    gap: 1rem;
  }

  .table-title {
    font-size: 1.35rem;
  }

  .search-box {
    max-width: 100%;
    width: 100%;
  }

  .name-cell {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.5rem;
  }

  .contact-avatar {
    font-size: 0.9rem;
    width: 2rem;
    height: 2rem;
  }

  .contact-name {
    font-size: 0.9rem;
  }

  .email-cell,
  .phone-cell {
    font-size: 0.85rem;
    gap: 0.5rem;
  }

  .action-buttons {
    gap: 0.5rem;
  }

  .action-edit,
  .action-delete {
    width: 2.5rem;
    height: 2.5rem;
  }

  :deep(.p-datatable .p-datatable-thead > tr > th) {
    padding: 1rem 0.75rem;
    font-size: 0.85rem;
  }

  :deep(.p-datatable .p-datatable-tbody > tr > td) {
    padding: 1rem 0.75rem;
  }

  :deep(.p-datatable .p-datatable-tbody > tr:hover) {
    transform: none;
  }

  :deep(.p-paginator) {
    flex-wrap: wrap;
    padding: 1rem;
    gap: 0.75rem;
  }

  :deep(.p-paginator .p-paginator-first),
  :deep(.p-paginator .p-paginator-prev),
  :deep(.p-paginator .p-paginator-next),
  :deep(.p-paginator .p-paginator-last) {
    min-width: 2.5rem;
    height: 2.5rem;
  }

  :deep(.p-paginator .p-paginator-pages .p-paginator-page) {
    min-width: 2.5rem;
    height: 2.5rem;
    font-size: 0.9rem;
  }
}

@media (max-width: 480px) {
  .contacts-page {
    padding: 1rem;
  }

  .table-title {
    font-size: 1.2rem;
  }

  .search-input {
    font-size: 0.9rem;
    padding: 0.75rem 1rem 0.75rem 2.5rem;
  }

  .empty-state {
    padding: 3rem 1rem;
  }

  .empty-icon {
    font-size: 3.5rem;
  }

  :deep(.p-tag) {
    font-size: 0.75rem;
    padding: 0.4rem 0.8rem;
  }

  .action-edit,
  .action-delete {
    width: 2.25rem;
    height: 2.25rem;
  }

  :deep(.p-datatable .p-datatable-thead > tr > th) {
    font-size: 0.75rem;
    padding: 0.75rem 0.5rem;
  }

  :deep(.p-datatable .p-datatable-tbody > tr > td) {
    padding: 0.75rem 0.5rem;
    font-size: 0.85rem;
  }
}
</style>
