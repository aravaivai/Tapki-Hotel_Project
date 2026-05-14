'use client'
import React, { useEffect, useState } from "react";
import Link from "next/link";
import Image from "next/image";

interface HeaderProps {
    showLogin?: boolean
    showProfile?: boolean
}

export default function Header({ showLogin = false, showProfile = false }: HeaderProps) {
    const [fullName, setFullName] = useState<string>('');

    useEffect(() => {
        const StoredUser = localStorage.getItem('user');
        if (StoredUser) {
            const ParsedUser = JSON.parse(StoredUser);
            setFullName(ParsedUser.FullName || '');
        }
    })
    return (
        <header className="flex items-center justify-between px-8 py-4 bg-gray-800 shadow-md w-full">
            <div className="flex items-center gap-4">
                <Link href='/'>
                    <div className="w-12 h-12 bg-gray-600 rounded-full border border-blue-600 flex items-center justify-center text-white font-bold">
                        <img src="/Logo.png" alt="Логотип" className="w-8 h-8" />
                    </div>
                </Link>
                <h1 className="text-2xl font-bold text-white">Отель "Tapki)))"</h1>
            </div>
            <nav className="flex gap-4">
                {showLogin && <Link href="/login" className="px-4 py-2 text-white border border-blue-600 rounded-lg hover:bg-blue-600 transition">
                    Вход
                </Link>}
                {showLogin && <Link href="/register" className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition">
                    Регистрация
                </Link>}
                {showProfile && <Link href="/">
                    <div className="w-10 h-10 bg-gray-600 rounded-full border border-blue-600 flex items-center justify-center text-white font-bold">
                        <img src="/ProfilePic.jpg" alt="профиль" className="h-full w-full object-cover rounded-full" loading="lazy" />
                    </div>
                    <div className="flex items-center justify-center text-center text-white">
                        {fullName}
                    </div>
                </Link>}

            </nav>
        </header>
    );
}