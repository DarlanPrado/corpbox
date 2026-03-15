<script setup lang="ts">
import { reactive, ref } from 'vue'

const { t } = useI18n({ useScope: 'local' })
const { login, register } = useAuthApi()

const loginForm = reactive({
  email: '',
  password: ''
})

const registerForm = reactive({
  name: '',
  email: '',
  password: ''
})

const loginPending = ref(false)
const registerPending = ref(false)
const feedback = ref('')
const feedbackType = ref<'success' | 'error'>('success')

function setFeedback(message: string, type: 'success' | 'error') {
  feedback.value = message
  feedbackType.value = type
}

async function submitLogin() {
  loginPending.value = true
  try {
    await login(loginForm)
    setFeedback(t('loginSuccess'), 'success')
  } catch (error) {
    const message = error instanceof Error ? error.message : t('unexpectedError')
    setFeedback(message, 'error')
  } finally {
    loginPending.value = false
  }
}

async function submitRegister() {
  registerPending.value = true
  try {
    await register(registerForm)
    setFeedback(t('registerSuccess'), 'success')
  } catch (error) {
    const message = error instanceof Error ? error.message : t('unexpectedError')
    setFeedback(message, 'error')
  } finally {
    registerPending.value = false
  }
}
</script>

<template>
  <article class="card">
    <h2>{{ t('title') }}</h2>
    <p class="description">{{ t('description') }}</p>

    <form class="form" @submit.prevent="submitLogin">
      <h3>{{ t('loginTitle') }}</h3>
      <label>
        {{ t('emailLabel') }}
        <input v-model="loginForm.email" type="email" required autocomplete="email" />
      </label>
      <label>
        {{ t('passwordLabel') }}
        <input v-model="loginForm.password" type="password" required minlength="8" autocomplete="current-password" />
      </label>
      <button type="submit" :disabled="loginPending">
        {{ loginPending ? t('loading') : t('loginButton') }}
      </button>
    </form>

    <form class="form" @submit.prevent="submitRegister">
      <h3>{{ t('registerTitle') }}</h3>
      <label>
        {{ t('nameLabel') }}
        <input v-model="registerForm.name" type="text" required minlength="2" maxlength="120" autocomplete="name" />
      </label>
      <label>
        {{ t('emailLabel') }}
        <input v-model="registerForm.email" type="email" required autocomplete="email" />
      </label>
      <label>
        {{ t('passwordLabel') }}
        <input v-model="registerForm.password" type="password" required minlength="8" autocomplete="new-password" />
      </label>
      <button type="submit" :disabled="registerPending">
        {{ registerPending ? t('loading') : t('registerButton') }}
      </button>
    </form>

    <p v-if="feedback" class="feedback" :class="feedbackType">
      {{ feedback }}
    </p>
  </article>
</template>

<i18n lang="json">
{
  "pt": {
    "title": "Autenticacao",
    "description": "Login e cadastro com validacao de request baseada no OpenAPI.",
    "loginTitle": "Entrar",
    "registerTitle": "Cadastrar",
    "nameLabel": "Nome",
    "emailLabel": "Email",
    "passwordLabel": "Senha",
    "loginButton": "Fazer login",
    "registerButton": "Criar conta",
    "loading": "Carregando...",
    "unexpectedError": "Ocorreu um erro inesperado.",
    "loginSuccess": "Login enviado com sucesso.",
    "registerSuccess": "Cadastro enviado com sucesso."
  },
  "en": {
    "title": "Authentication",
    "description": "Login and registration with OpenAPI-based request validation.",
    "loginTitle": "Sign in",
    "registerTitle": "Register",
    "nameLabel": "Name",
    "emailLabel": "Email",
    "passwordLabel": "Password",
    "loginButton": "Sign in",
    "registerButton": "Create account",
    "loading": "Loading...",
    "unexpectedError": "An unexpected error occurred.",
    "loginSuccess": "Login request sent successfully.",
    "registerSuccess": "Register request sent successfully."
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

h2,
h3 {
  margin: 0;
}

.description {
  margin: 0;
  color: #4b5563;
}

.form {
  display: grid;
  gap: 0.65rem;
  border-top: 1px solid #e4e4e7;
  padding-top: 0.8rem;
}

label {
  display: grid;
  gap: 0.25rem;
  font-size: 0.9rem;
}

input {
  border: 1px solid #cbd5e1;
  border-radius: 8px;
  padding: 0.55rem 0.65rem;
}

button {
  border: 0;
  background: #0f766e;
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

.feedback {
  margin: 0;
  border-radius: 8px;
  padding: 0.6rem 0.75rem;
}

.feedback.success {
  background: #dcfce7;
  color: #166534;
}

.feedback.error {
  background: #fee2e2;
  color: #991b1b;
}
</style>
