<script setup lang="ts">
const { t } = useI18n({ useScope: 'local' })
</script>

<template>
  <header class="header">
    <div>
      <p class="kicker">{{ t('kicker') }}</p>
      <p class="title">{{ t('title') }}</p>
    </div>

    <LanguageSwitcher />
  </header>
</template>

<i18n lang="json">
{
  "pt": {
    "kicker": "Workspace CorpBox",
    "title": "Portal de integracao"
  },
  "en": {
    "kicker": "CorpBox Workspace",
    "title": "Integration portal"
  }
}
</i18n>

<style scoped>
.header {
  max-width: 1100px;
  margin: 0 auto;
  padding: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.kicker {
  margin: 0;
  color: #57534e;
  font-size: 0.75rem;
  letter-spacing: 0.06em;
  text-transform: uppercase;
}

.title {
  margin: 0.3rem 0 0;
  font-size: 1.1rem;
  font-weight: 700;
}
</style>
