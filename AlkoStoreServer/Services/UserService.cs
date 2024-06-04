using AlkoStoreServer.Models;
using AlkoStoreServer.Repositories;
using AlkoStoreServer.Services.Interfaces;
using Azure.Core;
using Firebase.Auth.Repository;
using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using System.Collections.Immutable;

namespace AlkoStoreServer.Services
{
    public class UserService : IUserService
    {
        private readonly FirebaseAuth _firebaseAuth;

        private readonly FirestoreDb _firestoreDb;

        public UserService(
            FirebaseAuth firebaseAuth,
            FirestoreDb firestoreDb
        ) {
            _firebaseAuth = firebaseAuth;
            _firestoreDb = firestoreDb;

        }

        public async Task<string> GetUserNameByEmail(string email)
        {
            var kek = await _firestoreDb.Collection("users").WhereEqualTo("email", email).GetSnapshotAsync();

            var user = kek[0].ConvertTo<Dictionary<string, string>>();

            if (user.TryGetValue("firstName", out string value)) 
            { 
                return value;
            }

            return null;
        }
    }
}
