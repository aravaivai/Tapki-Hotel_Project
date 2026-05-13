import React from "react";
import Link from "next/link";
import Image from "next/image";

export default function Footer() {
    return (
        <footer className="bg-gray-800 text-white py-8 px-8">
        <div className="flex flex-col md:flex-row justify-between items-center opacity-70">
          <div className="w-12 h-12 bg-white rounded-full border border-blue-600 flex items-center justify-center text-white font-bold">
            <img src="/logo_VATT.jpg" alt="Логотип_ВАТТ" className="w-8 h-8" />
          </div>
          <p className="mr-auto ml-4">© 2026 Разработка: Захаров К.А. (СOGБПОУ ВАТТ)</p>
          <div className="flex gap-6 mt-4 md:mt-0 text-sm">
            <a href="#" className="hover:underline">Политика конфиденциальности</a>
            <a href="#" className="hover:underline">Правила проживания</a>
            <a href="/reviews" className="hover:underline">отзывы</a>
          </div>
        </div>
      </footer>
    );
}