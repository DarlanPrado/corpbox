<script setup lang="ts">
import { ref } from 'vue'

const { t } = useI18n({ useScope: 'local' })
const { getForecast } = useWeatherApi()

const pending = ref(false)
const entries = ref<Array<{ date?: string; summary?: string | null; temperatureC?: number | string }>>([])
const errorMessage = ref('')

async function loadForecast() {
  pending.value = true
  errorMessage.value = ''

  try {
    entries.value = await getForecast()
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : t('unexpectedError')
  } finally {
    pending.value = false
  }
}
</script>

<template>
  <article class="card">
    <h2>{{ t('title') }}</h2>
    <p class="description">{{ t('description') }}</p>

    <button type="button" :disabled="pending" @click="loadForecast">
      {{ pending ? t('loading') : t('loadButton') }}
    </button>

    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>

    <ul v-if="entries.length" class="list">
      <li v-for="(entry, index) in entries" :key="index">
        <strong>{{ entry.date ?? '-' }}</strong>
        <span>{{ entry.summary ?? t('noSummary') }}</span>
        <span>{{ entry.temperatureC ?? '-' }}C</span>
      </li>
    </ul>
  </article>
</template>

<i18n lang="json">
{
  "pt": {
    "title": "Previsao (resposta validada)",
    "description": "Exemplo de validacao runtime de resposta usando schema gerado do OpenAPI.",
    "loadButton": "Carregar previsao",
    "loading": "Carregando...",
    "unexpectedError": "Ocorreu um erro inesperado.",
    "noSummary": "Sem resumo"
  },
  "en": {
    "title": "Forecast (validated response)",
    "description": "Runtime response validation example using schema generated from OpenAPI.",
    "loadButton": "Load forecast",
    "loading": "Loading...",
    "unexpectedError": "An unexpected error occurred.",
    "noSummary": "No summary"
  }
}
</i18n>

<style scoped>
.card {
  background: #fff;
  border: 1px solid #d4d4d8;
  border-radius: 14px;
  padding: 1rem;
  display: grid;
  gap: 0.9rem;
}

h2 {
  margin: 0;
}

.description {
  margin: 0;
  color: #4b5563;
}

button {
  border: 0;
  background: #1d4ed8;
  color: #f8fafc;
  border-radius: 10px;
  padding: 0.65rem 0.85rem;
  font-weight: 600;
  cursor: pointer;
}

button:disabled {
  opacity: 0.75;
  cursor: wait;
}

.error {
  margin: 0;
  background: #fee2e2;
  color: #991b1b;
  border-radius: 8px;
  padding: 0.6rem 0.75rem;
}

.list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: grid;
  gap: 0.5rem;
}

.list li {
  display: grid;
  gap: 0.2rem;
  border: 1px solid #e4e4e7;
  border-radius: 10px;
  padding: 0.7rem;
}
</style>
