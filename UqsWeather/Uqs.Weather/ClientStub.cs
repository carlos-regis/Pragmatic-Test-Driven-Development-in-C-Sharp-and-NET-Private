using AdamTibi.OpenWeather;

namespace Uqs.Weather;

public class ClientStub : IClient
{
    public Task<OneCallResponse> OneCallAsync(decimal latitude,
                                              decimal longitude,
                                              IEnumerable<Excludes> excludes,
                                              Units unit)
    {
        const int DAYS = 7;
        OneCallResponse response = new()
        {
            Daily = new Daily[DAYS]
        };

        DateTime now = DateTime.Now;

        for (int i = 0; i < DAYS; i++)
        {
            response.Daily[i] = new Daily
            {
                Dt = now.AddDays(i),
                Temp = new Temp()
            };
            response.Daily[i].Temp.Day = Random.Shared.Next(-20, 55);
        }
        return Task.FromResult(response);
    }
}
