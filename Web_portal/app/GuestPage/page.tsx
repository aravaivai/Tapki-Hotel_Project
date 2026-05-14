'use client'
import React from "react";
import Header from "../components/header";
import Footer from "../components/footer";
import Link from "next/link";
import Image from "next/image";
import { createClient } from '../supabase'

export default function GuestPage() {
    return (
        <div className="flex flex-col h-screen bg-white">
            <Header showProfile />
            <main className="grow flex flex-col items-center justify-center">

            </main>
            <Footer />
        </div>
    )
}