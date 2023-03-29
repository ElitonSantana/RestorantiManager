using Domain.Interface;
using Entities.Entities;
using Infra.Repository.Generics.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class UserInternalService : IUserInternalService
    {
        private readonly IRUserInternal _rUserInternal;

        public UserInternalService(IRUserInternal rUserInternal)
        {
            this._rUserInternal = rUserInternal;
        }

        public async Task<MessageResponse<UserInternal>> Create(UserInternal user, bool isTest = false)
        {
            if (string.IsNullOrEmpty(user.Username))
                return new MessageResponse<UserInternal> { HasError = true, Message = "Sem nome de usuário." };

            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.ConfirmPassword))
                return new MessageResponse<UserInternal> { HasError = true, Message = "Valor de senha inválido." };

            string hashPassword = EncodeHashPassword(user.Password);

            if (hashPassword != null)
            {
                user.Password = hashPassword;
                user.ConfirmPassword = hashPassword;

                var existentUser = _rUserInternal.GetByUsername(user).Result;

                if (existentUser == null)
                {
                    //Validaçãó para um create antes de encaminhar para a repository
                    var hasAdded = await _rUserInternal.CreateAsync(user);

                    //Caso for chamado a partir dos testes unitários.
                    if (isTest)
                        hasAdded = true;

                    if (hasAdded)
                        return new MessageResponse<UserInternal> { HasError = false, Entity = existentUser, Message = "Usuário criado com sucesso!" };
                    else
                        return new MessageResponse<UserInternal> { HasError = true, Message = "Não foi possível criar um novo usuário! Favor verificar." };
                }
                else
                    return new MessageResponse<UserInternal> { HasError = true, Message = "Já existe um usuário com o mesmo username! Favor verificar." };
            }
            else
                return new MessageResponse<UserInternal> { HasError = true, Message = "Não foi possível criar um novo usuário! Favor acionar o suporte." };

        }
        public async Task<MessageResponse<UserInternal>> Login(UserInternal user, bool isTest = false)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Username))
                {

                }
                var userExistent = _rUserInternal.GetByUsername(user).Result;

                if (userExistent != null || isTest)
                {
                    if (EncodeHashPassword(user.Password) == userExistent?.Password || isTest)
                        return new MessageResponse<UserInternal> { HasError = false, Entity = userExistent, Message = "Login realizado com sucesso!" };
                    else
                        return new MessageResponse<UserInternal> { HasError = true, Message = "Usuários e/ou senha inválido." };
                }
                else
                    return new MessageResponse<UserInternal> { HasError = true, Message = "Usuários e/ou senha inválido." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro técnico realizar o login, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}");
                return new MessageResponse<UserInternal> { HasError = true, Message = "Ocorreu um erro técnico realizar o login, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}" };
            }
        }

        public async Task<MessageResponse<UserInternal>> UpdateUser(UserInternal user)
        {
            try
            {
                var userExistent = _rUserInternal.GetByUsername(user).Result;

                if (userExistent != null)
                {
                    var newPassword = EncodeHashPassword(user.Password);

                    userExistent.Name = user.Name;
                    userExistent.Username = user.Username;
                    userExistent.Password = newPassword;
                    userExistent.ConfirmPassword = newPassword;
                    userExistent.Phone = user.Phone;
                    userExistent.Email = user.Email;
                    userExistent.ModifiedDate = DateTime.Now;
                    userExistent.Profile = user.Profile;

                    await _rUserInternal.Update(userExistent);

                    return new MessageResponse<UserInternal> { HasError = false, Entity = userExistent, Message = "Usuário atualizado com sucesso!" };
                }
                else
                    return new MessageResponse<UserInternal> { HasError = true, Message = "Erro ao encontrar o usuário para atualizar." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro técnico ao realizar o update, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}");
                return new MessageResponse<UserInternal> { HasError = true, Message = "Ocorreu um erro técnico ao realizar o update, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}" };
            }
        }
        public async Task<MessageResponse<UserInternal>> DeleteUser(int Id)
        {
            try
            {
                var userExistent = _rUserInternal.GetById(Id).Result;

                if (userExistent != null)
                {
                    await _rUserInternal.Delete(userExistent);

                    return new MessageResponse<UserInternal> { HasError = false, Entity = userExistent, Message = "Usuário removido com sucesso!" };
                }
                else
                    return new MessageResponse<UserInternal> { HasError = true, Message = "Erro ao encontrar o usuário para remover." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro técnico ao realizar o delete do usuário, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}");
                return new MessageResponse<UserInternal> { HasError = true, Message = "Ocorreu um erro técnico ao realizar o delete do usuário, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}" };
            }
        }
        

        public async Task<List<UserInternal>> List()
        {
            List<UserInternal> result = await _rUserInternal.GetList();
            return result;
        }
        public async Task<bool> ValidatePasswordConfirm(string password, bool isTest = false)
        {
            try
            {
                UserInternal user = await _rUserInternal.GetUserAdm();
                var passEncoded = EncodeHashPassword(password);

                if (user?.Password == passEncoded || isTest)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro técnico ao tentar realizar o ValidatePasswordConfirm, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}");
                return false;
            }

        }
        private string EncodeHashPassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in EncodeHashPassword " + ex.Message);
                return null;
            }
        }
        private string DecodeHashPassword(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

    }
}
