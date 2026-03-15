<script setup lang="ts">
const { t } = useI18n({ useScope: 'local' })
</script>

<template>
  <section class="home">
    <header class="hero">
      <p class="eyebrow">{{ t('eyebrow') }}</p>
      <h1>{{ t('title') }}</h1>
      <p class="description">{{ t('description') }}</p>
    </header>

    <div class="grid">
      <AuthPanel />
      <ForecastPanel />
    </div>
  </section>
</template>

<i18n lang="json">
{
  "pt": {
    "eyebrow": "Sistema multilingue",
    "title": "CorpBox Web com i18n e OpenAPI",
    "description": "Traducoes por componente e payloads validados automaticamente a partir do Swagger do backend."
  },
  "en": {
    "eyebrow": "Multilingual system",
    "title": "CorpBox Web with i18n and OpenAPI",
    "description": "Per-component translations and payloads validated automatically from backend Swagger."
  }
}
</i18n>

<style scoped>
.home {
  display: grid;
  gap: 1.25rem;
}

.hero {
  background: #fff;
  border: 1px solid #d4d4d8;
  border-radius: 14px;
  padding: 1.25rem;
  box-shadow: 0 6px 24px rgba(20, 20, 30, 0.06);
}

.eyebrow {
  font-size: 0.8rem;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: #9a3412;
  margin: 0;
}

h1 {
  margin: 0.4rem 0;
  font-size: 1.85rem;
}

.description {
  margin: 0;
  color: #4b5563;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1rem;
}
</style>
