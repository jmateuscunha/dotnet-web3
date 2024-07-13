public static class SqlCommands
{
    public static string GetEthSepoliaAssets = @"select a.id, a.address, a.balance from public.asset a 
                                                join wallet w on a.wallet_id = w.id 
                                                join blockchain b on w.blockchain_id = b.id
                                                where b.name = 'Ethereum-Sepolia';";

    public static string UpdateEthSepoliaAssetsBalance = @"update public.asset 
                                                           set balance=CAST(@balance as numeric), updated_at=now() 
                                                           where id=@id;";

    public static string GetPendingEthSepoliaTransactions = @"select t.id as Id, t.from_address as FromAddress, t.hash as Hash
                                                              from public.transaction t 
                                                              where t.status = 'PENDING';";

    public static string ConfirmEthSepoliaTransaction = @"update public.transaction 
                                                          set status='CONFIRMED', block_number=cast(@blocknumber as numeric), updated_at=now() 
                                                          where id = @id;";

} 