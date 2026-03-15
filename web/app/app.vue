<template>
  <div class="shell">
    <NuxtRouteAnnouncer />
    <AppHeader />
    <main class="content">
      <NuxtPage />
    </main>
  </div>
</template>

<style scoped>
.shell {
  min-height: 100vh;
  background:
    radial-gradient(circle at top left, #f9d7aa 0%, transparent 40%),
    radial-gradient(circle at bottom right, #b5d9ff 0%, transparent 45%),
    #f4f3ef;
  color: #1f2937;
}

.content {
  margin: 0 auto;
  max-width: 1100px;
  padding: 1.25rem 1rem 2rem;
}
</style>
