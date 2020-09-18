using System;

namespace List {
    partial class List<T> where T : IEquatable<T>, IComparable<T> {
        private class Node<E> {
            private Node<E> _nextNode;
            private E _obj;

            public E Objeto {
                get {
                    return _obj;
                }
                set {
                    _obj = value;
                }
            }

            public Node<E> NextNode {
                get {
                    return _nextNode;
                }
                set {
                    _nextNode = value;
                }
            }

            public Node(E obj) {
                this._obj = obj;
                this.NextNode = null;
            }

            ~Node() {
                _obj = default;
            }

        }
    }
}
