import React from "react";
import { useState, useEffect } from "react";
import { createClient } from '../supabase'

export default function BalanceModal({ isOpen, onClose, onUpdate }: any) {
    const [amount, setAmount] = useState('');
    const [uID, setUID] = useState<number | null>(null);

    useEffect(() => {
        const StoredUser = localStorage.getItem('user');
        if (StoredUser) {
            const ParsedUser = JSON.parse(StoredUser);
            setUID(ParsedUser.UserID);
        }
    }, [isOpen]);

    const handlePay = async () => {
        if (!amount || !uID) return alert('Введите сумму');
        const supabase = createClient();

        const { data: clientData } = await supabase
            .from('Clients')
            .select('Balance')
            .eq('UserID', uID)
            .single();

        const currentBalance = clientData?.Balance || 0;
        const deposit = parseFloat(amount);

        const { error } = await supabase
            .from('Clients')
            .update({ Balance: currentBalance + deposit })
            .eq('UserID', uID);

        if (!error) {
            alert('Баланс пополнен!');
            setAmount('');
            if (typeof onUpdate === 'function') {
                onUpdate(uID);
            }
            onClose();
        } else {
            alert('Ошибка: ' + error.message);
        }
    };
    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 backdrop-blur-md bg-black/50 flex items-center justify-center p-4 animate-modal-overlay">
            <div className="bg-gray-800 p-6 rounded-lg text-White flex flex-col gap-4 mx-auto items-center justify-center animate-modal-content">
                <h3>Пополнение баланса</h3>
                <input
                    type="number"
                    placeholder="Введите сумму"
                    value={amount}
                    onChange={(e) => setAmount(e.target.value)}
                    className="bg-gray-900 text-white outline-none focus:ring-2 focus:scale-105 focus:ring-blue-500 p-3 rounded-lg transition-all duration-300"
                />
                <div className="flex gap-4 border border-gray-300 rounded-lg p-2">
                    <button type="button" className="bg-blue-500 text-white p-2 rounded-lg hover:bg-blue-700 hover:scale-105 transition-all duration-300" onClick={() => setAmount((prev) => (Number(prev) + 500).toString())}>+500</button>
                    <button type="button" className="bg-blue-500 text-white p-2 rounded-lg hover:bg-blue-700 hover:scale-105 transition-all duration-300" onClick={() => setAmount((prev) => (Number(prev) + 1000).toString())}>+1000</button>
                    <button type="button" className="bg-blue-500 text-white p-2 rounded-lg hover:bg-blue-700 hover:scale-105 transition-all duration-300" onClick={() => setAmount((prev) => (Number(prev) + 5000).toString())}>+5000</button>
                </div>
                <button className="bg-blue-500 w-50 text-white p-2 rounded-lg hover:bg-blue-700 hover:scale-105 transition-all duration-300" onClick={handlePay}>Пополнить</button>
                <button className="bg-blue-500 w-50 text-white p-2 rounded-lg hover:bg-blue-700 hover:scale-105 transition-all duration-300" onClick={onClose}>Отмена</button>
            </div>
        </div>
    );

}
