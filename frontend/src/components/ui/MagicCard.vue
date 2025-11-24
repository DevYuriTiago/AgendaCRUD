<template>
  <div
    :class="cn(
      'group relative flex flex-col justify-between overflow-hidden rounded-xl border bg-card text-card-foreground shadow',
      className
    )"
  >
    <div class="relative z-10 flex flex-col gap-4 p-6">
      <slot />
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
import { ref, onMounted, onUnmounted } from 'vue'
import { cn } from '@/lib/utils'

const props = defineProps({
  className: {
    type: String,
    default: '',
  },
})

const position = ref({ x: 0, y: 0 })
const cardRef = ref(null)

const handleMouseMove = (e) => {
  if (!cardRef.value) return
  const rect = cardRef.value.getBoundingClientRect()
  position.value = {
    x: e.clientX - rect.left,
    y: e.clientY - rect.top,
  }
}

// Note: In a real implementation, we'd attach this to the element ref
// For simplicity in this demo, we'll just use CSS hover for the main effect
// and this for the spotlight position if we were binding it to the div
</script>
