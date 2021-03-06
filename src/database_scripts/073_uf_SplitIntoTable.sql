CREATE FUNCTION [dbo].[uf_SplitIntoTable](@String varchar(4000), @Delimiter char(1))       
returns @temptable TABLE (Item varchar(4000))       
as       
begin       
    declare @idx int       
    declare @slice varchar(4000)       
       
    select @idx = 1       
         if len(@String)<1 or @String is null  return       

     while @idx!= 0       
     begin       
         set @idx = charindex(@Delimiter,@String)       
         if @idx!=0       
            set @slice = LTRIM (left(@String,@idx - 1))       
        else       
            set @slice = @String       
          
        if(len(@slice)>0)  
            insert into @temptable(Item) values(@slice)       
  
        set @String = LTRIM(right(@String,len(@String) - @idx))       
        if len(@String) = 0 break       
    end   
return       
end 

