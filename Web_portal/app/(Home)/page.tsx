import React from 'react';
import Link from 'next/link';
import Header from '../components/header';
import Footer from '../components/footer';

interface HotelContactInfo {
  address: string;
  phone: string;
  email: string;
}

export default function WelcomePage() {
  const contactData: HotelContactInfo = {
    address: "г. Смоленск, ул. Примерная, д. 10",
    phone: "+7 (999) 000-00-00",
    email: "info@tapki-hotel.ru"
  };

  return (
    <div className="flex flex-col min-h-screen">

      <Header showLogin/>

      <main className="grow">

        <section className="py-16 px-8 text-center bg-gray-50">
          <h2 className="text-4xl text-gray-800 font-extrabold mb-4">Добро пожаловать в уютный отдых</h2>
          <p className="max-w-2xl mx-auto text-black text-lg">
            небольшое описание отеля и услуг, не забыть напомнить о регистрации перед бронью
          </p>
        </section>

        <section className="py-12 px-8 bg-gray-800">
          <h3 className="text-2xl font-bold mb-6 text-center">Отель "Tapki)))"</h3>
          <div className="grid grid-cols-1 md:grid-cols-1 gap-6">
            <div className="aspect-video bg-gray-300 rounded-xl flex items-center justify-center text-gray-500 italic max-w-200 mx-auto"><img src="/hotelPhoto.JPG" alt="Фото 1" className="w-full h-full object-cover" /></div>
          </div>
        </section>

        <section className="py-12 px-8 bg-white">
          <div className="max-w-4xl mx-auto flex flex-col md:flex-row justify-between items-center gap-8">
            <div className="text-left">
              <h3 className="text-xl text-gray-600 font-bold mb-2">Где мы находимся?</h3>
              <p className="text-gray-600">{contactData.address}</p>
            </div>
            <div className="text-left">
              <h3 className="text-xl text-gray-600 font-bold mb-2">Связаться с нами</h3>
              <p className="text-gray-600">{contactData.phone}</p>
              <p className="text-gray-600">{contactData.email}</p>
            </div>
          </div>
        </section>
      </main>
      <Footer />
    </div>
  );
}