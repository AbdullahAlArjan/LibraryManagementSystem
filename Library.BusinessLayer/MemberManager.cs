using System;
using System.Data;
using Library.DataAccessLayer.Interfaces;
using Library.Entities;

namespace Library.BusinessLayer
{
    public class MemberManager
    {
        private readonly IMemberRepository _memberRepository;

        public MemberManager(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public int AddMember(Member member)
        {
            if (string.IsNullOrWhiteSpace(member.FullName))
                throw new ArgumentException("Member name is required.");

            return _memberRepository.AddMember(member);
        }

        public Member GetMemberById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid member ID.");

            return _memberRepository.GetMemberById(id);
        }

        public DataTable GetAllMembers()
        {
            return _memberRepository.GetAllMembers();
        }

        public bool UpdateMember(Member member)
        {
            if (member == null || member.MemberID <= 0)
                throw new ArgumentException("Invalid member data.");

            return _memberRepository.UpdateMember(member);
        }

        public bool DeleteMember(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid member ID.");

            return _memberRepository.DeleteMember(id);
        }
    }
}
