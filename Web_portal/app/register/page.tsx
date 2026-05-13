'use client'
import React, { useState } from 'react'
import { createClient } from '../supabase'
import Link from 'next/link'
import Header from '../components/header'
import Footer from '../components/footer'

export default function RegisterPage() {
const [showPassword, setShowPassword] = useState(false)

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
                            className="outline-none focus:border-blue-500 p-2 border border-gray-300 rounded-lg transition-colors ease-in-out duration-300"
                        />
                        <input
                            type={showPassword ? 'text' : 'password'}
                            placeholder="Пароль"
                            className="outline-none focus:border-blue-500 p-2 border border-gray-300 rounded-lg pr-10 mr-10 transition-colors ease-in-out duration-300"
                        />
                        <button
                            type="button"
                            onClick={togglePasswordVisibility}
                            className="absolute top-46 right-7 transform translate-y-[-50%] bg-blue-500 text-white p-2 rounded-lg hover:bg-blue-700 transition-colors duration-300"
                        >
                            {showPassword ? '🙈' : '👁️'}
                        </button>
                        <input
                            type="text"
                            placeholder="ФИО"
                            className="outline-none focus:border-blue-500 p-2 border border-gray-300 rounded-lg transition-colors ease-in-out duration-300"
                        />
                        <button
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