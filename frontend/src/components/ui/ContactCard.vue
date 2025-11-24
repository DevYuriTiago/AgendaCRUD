<template>
  <div
    ref="cardRef"
    @mousemove="handleMouseMove"
    :class="cn(
      'group relative flex flex-col justify-between overflow-hidden rounded-xl border bg-card text-card-foreground shadow-sm transition-all hover:shadow-md',
      className
    )"
  >
    <div class="p-6 relative z-10">
      <div class="flex items-center justify-between mb-4">
        <div class="flex items-center space-x-4">
          <div class="relative flex h-12 w-12 shrink-0 overflow-hidden rounded-full bg-secondary">
            <span class="flex h-full w-full items-center justify-center rounded-full bg-muted text-lg font-medium uppercase text-muted-foreground">
              {{ contact.name.charAt(0) }}
            </span>
          </div>
          <div>
            <h3 class="font-semibold leading-none tracking-tight">{{ contact.name }}</h3>
            <p class="text-sm text-muted-foreground">{{ contact.email }}</p>
          </div>
        </div>
        <div class="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
          <button @click="$emit('edit', contact)" class="p-2 hover:bg-secondary rounded-full transition-colors cursor-pointer">
            <i class="pi pi-pencil text-foreground/70"></i>
          </button>
          <button @click="$emit('delete', contact)" class="p-2 hover:bg-destructive/10 rounded-full transition-colors cursor-pointer">
            <i class="pi pi-trash text-destructive"></i>
          </button>
        </div>
      </div>
      
      <div class="space-y-2">
        <div class="flex items-center text-sm text-muted-foreground">
          <i class="pi pi-phone mr-2 h-4 w-4"></i>
          {{ contact.phone }}
        </div>
      </div>
    </div>
    
    <div
      class="pointer-events-none absolute -inset-px opacity-0 transition duration-300 group-hover:opacity-100"
      :style="{
        background: `radial-gradient(600px circle at ${position.x}px ${position.y}px, rgba(255,255,255,0.1), transparent 40%)`,
      }"
    />
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { cn } from '@/lib/utils'

defineProps({
  contact: {
    type: Object,
    required: true,
  },
  className: {
    type: String,
    default: '',
  },
})

defineEmits(['edit', 'delete'])

const cardRef = ref(null)
const position = ref({ x: 0, y: 0 })

const handleMouseMove = (e) => {
  if (!cardRef.value) return
  const rect = cardRef.value.getBoundingClientRect()
  position.value = {
    x: e.clientX - rect.left,
    y: e.clientY - rect.top,
  }
}
</script>
