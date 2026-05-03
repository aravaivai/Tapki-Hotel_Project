'use client'
import { useEffect, useState } from 'react'
import { createClient } from './supabase' // проверь путь до файла

export default function Home() {
  const [rooms, setRooms] = useState<any[]>([])
  const supabase = createClient()

  useEffect(() => {
    const fetchRooms = async () => {
      const { data } = await supabase.from('Rooms').select('*')
      if (data) setRooms(data)
    }
    fetchRooms()
  }, [])

  return (
    <main style={{ padding: '20px' }}>
      <h1>Номера отеля "Тапки"</h1>
      <ul>
        {rooms.map((room) => (
          <li key={room.RoomID}>
            {room.RoomName} — {room.Price} руб.
          </li>
        ))}
      </ul>
    </main>
  )
}