using List;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Generic_Class;

namespace List {

    class Node<T> {
        private Node<T> _nextNode;
        private T _obj;

        public T Objeto {
            get {
                return _obj;
            }
            set {
                _obj = value;
            }
        }

        public Node<T> NextNode {
            get {
                return _nextNode;
            }
            set {
                _nextNode = value;
            }
        }

        public Node(T obj) {
            this._obj = obj;
            this.NextNode = null;
        }

        ~Node() {
            Console.WriteLine("Dead node");
            _obj = default;
        }
    }
    /// <summary>
    /// ListaOrdenada es una estructura de datos dinámica parametrizada que tiene como característica
    /// principal el evitar el duplicado de datos. Ordena los datos con la implementación del método
    /// CompareTo() del tipo que se parametrice. 
    /// </summary>
    /// <typeparam name="T">Clase roja o tipo de dato que implemente IEquatable e IComparable</typeparam>
    public class ListaOrdenada<T> where T : IEquatable<T>, IComparable<T> {
        public delegate bool Condicion<in U>(U aT);
        private Node<T> head;
        /// <summary>
        /// Accesa a cualquier elemento de la lista.
        /// </summary>
        /// <param name="index">Indice para accesar a la lista</param>
        /// <returns>El objeto del nodo accesado</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public T this[int index] {
            get {
                if (EstaVacia || index >= Size)
                    throw new IndexOutOfRangeException("No existe el indice indicado.");
                Node<T> currentNode = head;
                var i = 0;
                while (i++ < index) currentNode = currentNode.NextNode;
                return currentNode.Objeto;
            }
        }
        /// <summary>
        /// Devuelve el tamaño de la lista
        /// </summary>
        public int Size {
            private set;
            get;
        }
        /// <summary>
        /// Devuelve true si la lista esta vacia o false en caso contrario
        /// </summary>
        public bool EstaVacia {
            get => head == null;
        }
        public ListaOrdenada<T> RetornaSi(Condicion<T> condicion) {
            ListaOrdenada<T> returnLista = new ListaOrdenada<T>();
            foreach (T item in this) {
                if (condicion.Invoke(item)) {
                    returnLista.Agregar(item);
                }
            }
            return returnLista;
        }
        /// <summary>
        /// Limpia la lista y elimina todos los elementos
        /// </summary>
        public void BorrarSi(Condicion<T> condicion) {
            ForEach(v => {
                if (condicion.Invoke(v)) {
                    Borrar(v);
                }
            });
        }
        public void AgregarSi(Condicion<T> condicion, params T[] objs) {
           foreach(T t in objs) {
                if (condicion.Invoke(t)) {
                    Agregar(t);
                }
            }
        }
        public void Limpiar() {
            head = null;
            Size = 0;
        }
        /// <summary>
        /// Crea una lista ordenada vacia.
        /// </summary>
        public ListaOrdenada() {
            Limpiar(); //Mismo codigo, inecesario repetirlo
        }
        /// <summary>
        /// Crea una lista ordenada con N objetos a insertar directamente mediante
        /// el constructor
        /// </summary>
        /// <param name="objects"></param>
        public ListaOrdenada(params T[] objects) : this() {
            Agregar(objects);
        }
        /// <summary>
        /// Borra un elemento de la lista.
        /// </summary>
        /// <param name="obj">El elemento que se desea borrar</param>
        /// <returns>El elemento borrado</returns>
        public T Borrar(T obj) {
            if (EstaVacia) throw new Exception("Lista vacia. No se puede borrar ningun elemento.");
            Node<T> currentNode = head, previousNode = null;
            while (currentNode != null) {
                if (currentNode.Objeto.CompareTo(obj) == 1)
                    break;
                if (currentNode.Objeto.Equals(obj)) {
                    if (previousNode != null) { //int
                        previousNode.NextNode = currentNode.NextNode;
                    } else { //h
                        head = head.NextNode;
                    }
                    Size--;
                    return currentNode.Objeto;
                }
                previousNode = currentNode;
                currentNode = currentNode.NextNode;
            }
            throw new Exception("No se encontro el elemento a borrar");
        }
        public void Borrar(params T[] objs) {
            foreach (T i in objs) {
                Borrar(i);
            }
        }
        /// <summary>
        /// Busca un elemento en la lista y lo devuelve si lo encontró.
        /// </summary>
        /// <param name="obj">Objeto a buscar</param>
        /// <returns>Objeto encontrado</returns>
        public T Buscar(T obj) {

            if (EstaVacia) {
                throw new Exception("Lista vacia");
            }

            Node<T> actual = head;
            while (actual != null) {
                if (actual.Objeto.Equals(obj)) {
                    return actual.Objeto;
                } else if (obj.CompareTo(actual.Objeto) == -1) {
                    break;
                } else actual = actual.NextNode;
            }

            throw new Exception("No encontrado");
        }
        /// <summary>
        /// Iterador de la lista. Recorre del principio a fin
        /// </summary>
        /// <returns>Cada elemento de la lista.</returns>
        public IEnumerator GetEnumerator() {
            if (EstaVacia)
                yield break;
            Node<T> currentNode = head;
            while (currentNode != null) {
                yield return currentNode.Objeto;
                currentNode = currentNode.NextNode;
            }

        }
        /// <summary>
        /// Agrega un elemento a la lista.
        /// </summary>
        /// <param name="obj">Objeto a agregar a la lista. <see cref="Agregar(T[])"/></param>
        /// <exception cref="Exception">No admite duplicados</exception>
        public void Agregar(T obj) {
            Node<T> nuevoNodo = new Node<T>(obj), prevNode = null, currentNode = head;
            while (currentNode != null && currentNode.Objeto.CompareTo(obj) <= 0) {
                if (obj.Equals(currentNode.Objeto)) throw new Exception("No se permiten duplicados en esta lista.");
                prevNode = currentNode;
                currentNode = currentNode.NextNode;
            }
            if (prevNode == null) { //h
                nuevoNodo.NextNode = head;
                head = nuevoNodo;
            } else { //int
                prevNode.NextNode = nuevoNodo;
                nuevoNodo.NextNode = currentNode;
            }
            Size++;
        }
        /// <summary>
        /// Agrega varios objetos a la vez a la lista.
        /// </summary>
        /// <param name="objs">Objetos a agregar a la lista (varios)</param>
        public void Agregar(params T[] objs) {
            foreach (T i in objs) {
                Agregar(i);
            }
        }
        /// <summary>
        /// Metodo que ejecuta las acciones en cada objeto
        /// </summary>
        /// <param name="action">Accion a ejecutar</param>
        public void ForEach(Action<T> action) {
            foreach (T i in this) {
                action.Invoke(i);
            }
        }
        /// <summary>
        /// Limpia la lista y elimina todo (GC)
        /// </summary>
        ~ListaOrdenada() {
            Limpiar();
        }
    }
}


    class Program
    {
        static void Main(string[] args)
        {

        GenericClass claseGenerica = new GenericClass();

        claseGenerica.AddProperty(new Property("intEdad", 20));
        claseGenerica.AddProperty(new Property("strNombre", "Alan Pena"));
        claseGenerica.AddProperty(new Property("dblPeso", 90 ));
        claseGenerica.AddProperty(new Property("chrSexo", 'M'));
        claseGenerica.AddProperty(new Property("blnMayorDeEdad", claseGenerica.GetProperty
            ("intEdad") >=18 ));


        foreach(Property prop in claseGenerica) {
            Console.WriteLine(prop.Value);
        }




    }
}

