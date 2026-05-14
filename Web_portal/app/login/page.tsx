'use client'
import React, { useState } from 'react'
import { createClient } from '../supabase'
import Link from 'next/link'
import Header from '../components/header'
import Footer from '../components/footer'

export default function LoginPage() {
  const [login, setLogin] = useState('')
  const [password, setPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const supabase = createClient()

  const togglePasswordVisibility = () => {
    setShowPassword((prev) => !prev);
  };

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    const { data, error } = await supabase
      .from('Users')
      .select('*')
      .eq('Login', login)
      .eq('Password', password)
      .eq('Role', 'Гость')
      .single()

    if (data) {
      localStorage.setItem('user', JSON.stringify(data));
      window.location.href = '/GuestPage';
    } else {
      alert('Неверный логин или пароль')
    }
  }

  return (

    <div className="flex flex-col h-screen bg-white">
      <Header />
      <main className="grow flex flex-col items-center justify-center">
        <div className="bg-gray-800 p-8 rounded-lg shadow-lg mt-30 position relative">
          <h2 className="text-4xl font-bold text-center mb-8">Вход в систему</h2>
          <form className="flex flex-col gap-4">
            <input
              type="text"
              placeholder="Логин"
              value={login}
              onChange={(e) => setLogin(e.target.value)}
              className="outline-none focus:border-blue-500 focus:scale-110 p-2 border border-gray-300 rounded-lg transition-all ease-in-out duration-300"
            />
            <input
              type={showPassword ? 'text' : 'password'}
              placeholder="Пароль"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="outline-none focus:border-blue-500 focus:scale-105 p-2 border border-gray-300 rounded-lg pr-10 transition-all ease-in-out duration-300"
            />
            <button
              type="button"
              onClick={togglePasswordVisibility}
              className="absolute top-46 right-10 transform translate-y-[-50%] bg-none p-1 hover:bg-gray-200/15 rounded-lg"
            >
              {showPassword ? '🙈' : '👁️'}
            </button>
            <button
              onClick={handleLogin}
              className="bg-blue-500 text-white p-2 rounded-lg hover:bg-blue-700 transition-colors duration-300"
            >
              Войти
            </button>
            <p className='text-center text-1xl text-gray-500'>нет аккаунта?</p>
            <Link href="/register" className="text-center hover:underline text-xs text-gray-500">Зарегистрируйтесь</Link>
          </form>
        </div>
      </main>
      <Footer />
    </div>
  )
}