<script setup lang="ts">
const switchLocalePath = useSwitchLocalePath()
const { locale, t } = useI18n({ useScope: 'local' })

const locales = [
  { code: 'pt', key: 'portuguese' },
  { code: 'en', key: 'english' }
]
</script>

<template>
  <nav class="switcher" :aria-label="t('label')">
    <NuxtLink
      v-for="item in locales"
      :key="item.code"
      :to="switchLocalePath(item.code)"
      class="button"
      :class="{ active: locale === item.code }"
    >
      {{ t(item.key) }}
    </NuxtLink>
  </nav>
</template>

<i18n lang="json">
{
  "pt": {
    "label": "Seletor de idioma",
    "portuguese": "PT",
    "english": "EN"
  },
  "en": {
    "label": "Language switcher",
    "portuguese": "PT",
    "english": "EN"
  }
}
</i18n>

<style scoped>
.switcher {
  display: inline-flex;
  background: #e7e5e4;
  padding: 0.25rem;
  border-radius: 999px;
}

.button {
  text-decoration: none;
  color: #374151;
  border-radius: 999px;
  padding: 0.3rem 0.7rem;
  font-size: 0.9rem;
}

.button.active {
  background: #0f172a;
  color: #f8fafc;
}
</style>
