package com.example.appprogettott

import android.os.AsyncTask
import android.util.Log
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.InetSocketAddress
import java.net.Socket

class Client: AsyncTask<String?, String?, String?>() {

    override fun doInBackground(vararg params: String?): String? {
        var socket: Socket? = null

        try {
            // crea la socket e connettiti al server
            socket = Socket("192.168.178.149", 25565)
            Log.d("SERVER_INFO",socket.toString())

            // crea lo stream di output e invia il messaggio al server
            // val out = PrintWriter(BufferedWriter(OutputStreamWriter(socket.getOutputStream())), true)
            // out.println(message)

            // crea lo stream di input e leggi la risposta del server
            val inStream = BufferedReader(InputStreamReader(socket.getInputStream()))
            val response = inStream.readLine()
            Log.d("SUPER_PALLE","Server response: $response")

        } catch (e: Exception) {
            e.printStackTrace()
        } finally {
            // chiudi la socket
            socket?.close()
        }
        return null
    }
}