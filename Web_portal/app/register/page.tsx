'use client'
import React, { useState } from 'react'
import { createClient } from '../supabase'
import Link from 'next/link'
import Header from '../components/header'
import Footer from '../components/footer'
import { title } from 'process'

export default function RegisterPage() {
const [showPassword, setShowPassword] = useState(false)
const supabase = createClient()
const [login, setLogin] = useState('')
const [password, setPassword] = useState('')
const [fullName, setFullName] = useState('')

const regUser = async (e: React.FormEvent) => {
    e.preventDefault()
    const {data, error} = await supabase
    .from('Users')
    .select('*')
    .eq('Login', login)

    if(data && data.length > 0){
        alert('Пользователь с таким логином уже существует')
    }
    else if(login != '' && password != '' && fullName != ''){
        const {data, error} = await supabase
        .from('Users')
        .insert({Login: login, Password: password, FullName: fullName, Role: 'Гость'})

        alert('Вы зарегистрировались!')
        window.location.href = '/login'
    }
    else{
        alert('Заполните все поля!')
    }
}

const togglePasswordVisibility = () => {
  setShowPassword((prev) => !prev);
};

    return (
        <div className="flex flex-col h-screen bg-white">
            <Header />
            <main className="grow flex flex-col items-center justify-center">
                <div className="bg-gray-800 p-8 rounded-lg shadow-lg mt-30 position relative">
                    <h2 className="text-4xl font-bold text-center mb-8">Регистрация</h2>
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
                            className="outline-none focus:border-blue-500 focus:scale-105 p-2 border border-gray-300 rounded-lg transition-all ease-in-out duration-300"
                        />
                        <button
                            type="button"
                            onClick={togglePasswordVisibility}
                            className="absolute top-46 right-10 transform translate-y-[-50%] bg-none p-1 hover:bg-gray-200/15 rounded-lg"
                        >
                            {showPassword ? '🙈' : '👁️'}
                        </button>
                        <input
                            type="text"
                            placeholder="ФИО"
                            value={fullName}
                            onChange={(e) => setFullName(e.target.value)}
                            className="outline-none focus:border-blue-500 focus:scale-110 p-2 border border-gray-300 rounded-lg transition-all ease-in-out duration-300"
                        />
                        <button
                            onClick={regUser}
                            className="bg-blue-500 text-white p-2 rounded-lg hover:bg-blue-700 transition-colors duration-300"
                        >
                            Зарегистрироваться
                        </button>
                    </form>
                </div>
            </main>
            <Footer />
        </div>
    );
}